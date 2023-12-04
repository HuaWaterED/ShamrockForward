using Shamrock_Forward.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Shamrock_Forward.WebSocketClient
{
    internal class WebSocketClient
    {
        public ClientWebSocket clientWebSocket=new();
        public Action<string> OnMessage=(m)=> { };
        public string uriString=string.Empty;
        public WebSocketClient SetURI(string uriString)
        {
            this.uriString = uriString;
            return this;
        }
        public WebSocketClient SetOnMessage(Action<string> OnMessage)
        {
            this.OnMessage = OnMessage;
            return this;
        }
        public WebSocketClient AddHeader(string headerName,string headerValue)
        {
            clientWebSocket.Options.SetRequestHeader(headerName: headerName, headerValue: headerValue);
            return this;
        }
        public void Start()
        {
            clientWebSocket.Options.Proxy = null;
            Uri uri = new(uriString);
            clientWebSocket.ConnectAsync(uri, CancellationToken.None).Wait();
            Thread rec_msg = new(() =>
            {
                while (true)
                {
                    ArraySegment<byte> bytesReceived = new(new byte[Config.Instance.messageLength]);
                    WebSocketReceiveResult result = clientWebSocket.ReceiveAsync(bytesReceived,CancellationToken.None).Result;
                    string rec_Message = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                    OnMessage(rec_Message);
                }
            });
            rec_msg.IsBackground = true;
            rec_msg.Start();
        }
    }
}
