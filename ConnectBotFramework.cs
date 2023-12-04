using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shamrock_Forward.Instacne;
using Shamrock_Forward.Library.Request;
using Shamrock_Forward.Library.Event;
using Shamrock_Forward.Log;
using Shamrock_Forward.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Shamrock_Forward.Library.Respone;
using System.Net.WebSockets;
using Shamrock_Forward.Library.Data;
using Shamrock_Forward.WebSocketClient;

namespace Shamrock_Forward
{
    internal class ConnectBotFramework:InstanceAny<ConnectBotFramework>
    {
        public List<Link> clientLinks = new();
        public List<WebSocketClientLink> webSocketClientLinks = new();
        WebSocketServer thislink=new("ws://0.0.0.0:1286");
        public void Init(string addr,string port)
        {
            thislink = new($"ws://{addr}:{port}");
            thislink.Start(WebSocketConnection);
            for (int i = 0; i < Config.Instance.ws_Revs.Count; i++)
            {
                ws_rev ws_Rev = Config.Instance.ws_Revs[i];
                Thread newWebSocketClientLink = new(() => 
                {
                    new WebSocketClientLink(ws_Rev, webSocketClientLinks);
                });
                newWebSocketClientLink.IsBackground = true;
                newWebSocketClientLink.Start();
            }
        }

        void WebSocketConnection<T>(T webSocketConnection) where T : IWebSocketConnection
        {
            _ = new Link(webSocketConnection, clientLinks);
            //public void Close();
            //public void Close(int code);
            //public Task Send(string message);
            //public Task Send(byte[] message);
            //public Task SendPing(byte[] message);
            //public Task SendPong(byte[] message);
            //webSocketConnection. ConnectionInfo;
            //webSocketConnection. IsAvailable;
            //webSocketConnection.OnOpen = () => { };
            //webSocketConnection.OnClose = () => { };
            //webSocketConnection.OnMessage = (m) => { };
            //webSocketConnection.OnBinary = (m) => { };
            //webSocketConnection.OnPing = (m) => { };
            //webSocketConnection.OnPong = (m) => { };
            //webSocketConnection.OnError = (m) => { };
        }

        public string FakeReturnWithMessageID(Request? request)
        {
            Respone fakeReturn = new();
            fakeReturn.status = Library.Respone.Status.ok;
            fakeReturn.retcode = 0;
            fakeReturn.data.time = Utils.Utils.GetTimeStamp();
            fakeReturn.data.message_id = -2147483647;
            fakeReturn.echo = request!.echo;
            return JsonConvert.SerializeObject(fakeReturn);
        }
        public string FakeReturnWithVoid(Request? request)
        {
            Respone fakeReturn = new();
            fakeReturn.status = Library.Respone.Status.ok;
            fakeReturn.retcode = 0;
            fakeReturn.echo = request!.echo;
            return JsonConvert.SerializeObject(fakeReturn);
        }
    }
    class WebSocketClientLink
    {
        ws_rev ws_Rev;
        ClientWebSocket webSocketClient;
        List<WebSocketClientLink> webSocketClientLinks;
        string connectAddress => $@"ws://{ws_Rev.ws_revIP}:{ws_Rev.ws_revPort}{ws_Rev.ws_revEndPoint}";
        public WebSocketClientLink(ws_rev ws_Rev, List<WebSocketClientLink> webSocketClientLinks)
        {
            this.ws_Rev = ws_Rev;
            this.webSocketClientLinks = webSocketClientLinks;
            this.webSocketClientLinks.Add(this);
            while (true)
            {
                try
                {
                    webSocketClient = new();
                    webSocketClient.Options.SetRequestHeader("Authorization", $"Bearer {Config.Instance.accessToken}");
                    webSocketClient.Options.SetRequestHeader("User-Agent", $"Shamrock/1.0.6-dev.3a0dc41");
                    webSocketClient.Options.SetRequestHeader("X-Client-Role", $"Universal");
                    webSocketClient.Options.SetRequestHeader("X-Impl", $"Shamrock");
                    webSocketClient.Options.SetRequestHeader("X-OneBot-Version", $"11");
                    webSocketClient.Options.SetRequestHeader("X-QQ-Version", $"android 8.9.78");
                    webSocketClient.Options.SetRequestHeader("X-Self-ID", $"{Config.Instance.botQQ}");
                    //.SetURI(connectAddress)
                    //.SetOnMessage((m) => Forward.Instance.Forward2Shamrock(m));
                    //webSocketClient.clientWebSocket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,statusDescription: "",cancellationToken: CancellationToken.None).Wait();
                    webSocketClient.Options.Proxy = null;
                    Uri uri = new(connectAddress);
                    webSocketClient.ConnectAsync(uri, CancellationToken.None).Wait();
                    while (true)
                    {
                        ArraySegment<byte> bytesReceived = new(new byte[Config.Instance.messageLength]);
                        WebSocketReceiveResult result = webSocketClient.ReceiveAsync(bytesReceived, CancellationToken.None).Result;
                        string rec_Message = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                        Forward.Instance.Forward2Shamrock(rec_Message);
                    }
                }
                catch(Exception e)
                {
                    LogSystem.Log($"链接：{connectAddress}已断开，原因如下：\n{e}",Log.LogLevel.Error);
                    LogSystem.Log($@"正在尝试重新连接{connectAddress}", Log.LogLevel.Warn);
                }
            }
        }
        public void SendMessage(string messageContent)
        {
            try
            {
                ArraySegment<byte> bytesToSend = new(Encoding.UTF8.GetBytes(messageContent));
                SendMessage(bytesToSend.ToArray());
            }
            catch(Exception e)
            {
                LogSystem.Log($"发送消息到{connectAddress}失败！消息内容如下：\n\n{messageContent}\\n原因如下：\n    {e}",Log.LogLevel.Error);
            }
        }
        public void SendMessage(byte[] bytes)
        {
            webSocketClient.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    class Link
    {
        IWebSocketConnection webSocketConnection;
        List<Link> webSocketConnections;
        public Link(IWebSocketConnection webSocketConnection,List<Link> webSocketConnections) 
        { 
            this.webSocketConnection = webSocketConnection;
            this.webSocketConnections = webSocketConnections;
            this.webSocketConnection.OnOpen = OnOpen;
            this.webSocketConnection.OnClose = OnClose;
            this.webSocketConnection.OnMessage = OnMessage;
            this.webSocketConnection.OnBinary = OnBinary;
            this.webSocketConnection.OnPing = OnPing;
            this.webSocketConnection.OnPong = OnPong;
            this.webSocketConnection.OnError = OnError;
        }
        public void OnOpen()
        {
            LogSystem.Log("新的连接！具体信息如下：", consoleColor: ConsoleColor.Green);
            foreach (var item in webSocketConnection.ConnectionInfo.Headers)
            {
                LogSystem.Log($"{item.Key}：{item.Value}", consoleColor: ConsoleColor.Green);
            }
            string wsPathToken = webSocketConnection.ConnectionInfo.Path;
            wsPathToken = wsPathToken[(wsPathToken.IndexOf('?')+1)..];
            if (wsPathToken!.StartsWith("access_token="))wsPathToken = wsPathToken[13..];
            else if (wsPathToken!.StartsWith("ticket="))wsPathToken = wsPathToken[7..];
            else if (wsPathToken!.StartsWith("Authorization="))wsPathToken = wsPathToken[14..];

            webSocketConnection.ConnectionInfo.Headers.Add("CustomAddToken",wsPathToken);
            string accessToken = string.Empty;
            if (webSocketConnection.ConnectionInfo.Headers.TryGetValue("access_token", out accessToken!)) { }
            else if (webSocketConnection.ConnectionInfo.Headers.TryGetValue("ticket", out accessToken!)) { }
            else if (webSocketConnection.ConnectionInfo.Headers.TryGetValue("Authorization", out accessToken!)) { }
            else if (webSocketConnection.ConnectionInfo.Headers.TryGetValue("CustomAddToken", out accessToken!)) { }
            else
            {
                ConnectFailed(accessToken);
                return;
            }
            if (accessToken!.StartsWith("Bearer "))
            {
                accessToken = accessToken[7..];
            }
            if (accessToken!.StartsWith("Token "))
            {
                accessToken = accessToken[6..];
            }
            string[] tokenList = accessToken.Split(new char[] { ',', '|', '，' });
            foreach (string token in tokenList)
            {
                if (token == Config.Instance.accessToken)
                {
                    webSocketConnections.Add(this);
                    LogSystem.Log($"WSServer连接({webSocketConnection.ConnectionInfo.ClientIpAddress}:{webSocketConnection.ConnectionInfo.ClientPort})", consoleColor: ConsoleColor.Yellow);

                    Forward.Instance.Forward2BotFrame(JsonConvert.SerializeObject(new Heartbeat(Utils.Utils.GetTimeStamp())));
                    return;
                }
            }
            ConnectFailed(accessToken);
        }

        private void ConnectFailed(string accessToken)
        {
            LogSystem.Log($"WSServer连接错误({webSocketConnection.ConnectionInfo.ClientIpAddress}:{webSocketConnection.ConnectionInfo.ClientPort}) 没有提供正确的token, {accessToken}。", Log.LogLevel.Error);
            webSocketConnection.Close();
        }

        public void OnClose()
        {
            LogSystem.Log("现有连接已断开！具体信息如下：",Log.LogLevel.Error);
            foreach (var item in webSocketConnection.ConnectionInfo.Headers)
            {
                LogSystem.Log($"{item.Key}：{item.Value}",Log.LogLevel.Error);
            }
            webSocketConnections.Remove(this);
        }
        public void SendMessage(string messageContent)
        {
            webSocketConnection.Send(messageContent);
        }
        public void SendMessage(byte[] bytes)
        {
            webSocketConnection.Send(bytes);
        }
        public void OnMessage(string message)
        {
            Forward.Instance.Forward2Shamrock(message);
        }
        public void OnBinary(byte[] bytes)
        {

        }
        public void OnPing(byte[] bytes)
        {
            StringBuilder pinginfo = new();
            foreach (var item in bytes)
            {
                pinginfo.Append($"{item} ");
            }
            LogSystem.Log($"收到Ping信息！信息如下：{pinginfo}",consoleColor:ConsoleColor.Magenta);
            //ForwardMessage.Instance.Forward2Shamrock(bytes);
            webSocketConnection.SendPong(bytes);
            LogSystem.Log($"已回复Pong信息！信息如下：{pinginfo}", consoleColor: ConsoleColor.Magenta);
        }
        public void OnPong(byte[] bytes)
        {
            LogSystem.Log("收到Pong信息！");
        }
        public void OnError(Exception e)
        {

        }









        //public Action OnOpen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Action OnClose { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Action<string> OnMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Action<byte[]> OnBinary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Action<byte[]> OnPing { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Action<byte[]> OnPong { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //public Action<Exception> OnError { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //public IWebSocketConnectionInfo ConnectionInfo => throw new NotImplementedException();

        //public bool IsAvailable => throw new NotImplementedException();

        //public void Close()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Close(int code)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task Send(string message)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task Send(byte[] message)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SendPing(byte[] message)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SendPong(byte[] message)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
