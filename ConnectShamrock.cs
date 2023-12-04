using Shamrock_Forward.Instacne;
using Shamrock_Forward.Log;
using Shamrock_Forward.WebSocketClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Shamrock_Forward
{
    internal class ConnectShamrock: InstanceAny<ConnectShamrock>
    {
        public WebSocketClient.WebSocketClient toShamrock = new();
        string connectAddress => $"ws://{Config.Instance.shamrockIP}:{Config.Instance.shamrockPort}{Config.Instance.shamrockEndPoint}";
        public void Init(string webSocketUrl)
        {
            toShamrock.AddHeader("Authorization", Config.Instance.accessToken)
                .SetURI(webSocketUrl)
                .SetOnMessage(OnMessage)
                .Start();
        }
        public void OnMessage(string message)
        {
            Forward.Instance.Forward2BotFrame(message);
        }
        public void SendMessage(string messageContent)
        {
            try
            {
                ArraySegment<byte> bytesToSend = new(Encoding.UTF8.GetBytes(messageContent));
                SendMessage(bytesToSend.ToArray());
            }
            catch (Exception e)
            {
                LogSystem.Log($"发送消息到{connectAddress}失败！消息内容如下：\n\n{messageContent}\\n原因如下：\n    {e}", LogLevel.Error);
            }
        }
        public void SendMessage(byte[] bytes)
        {
            toShamrock.clientWebSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
