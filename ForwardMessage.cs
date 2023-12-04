using Shamrock_Forward.Instacne;
using Shamrock_Forward.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Timers;
using Timer = System.Timers.Timer;
using Shamrock_Forward.Library.Data;
using Action = Shamrock_Forward.Library.Data.Action;
using Shamrock_Forward.Library.Request;
using System.Net.Http.Headers;

namespace Shamrock_Forward
{
    internal class Forward:InstanceAny<Forward>
    {
        public Forward() 
        {
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Elapsed += (object? sender, ElapsedEventArgs e) => 
            {
                ForwardMessageList2Shamrock();
            };
            timer.Start();
        }
        long nextSendMessageTime = -1;
        List<string> messages = new();
        //int32最小值：-2147483648，那就最小值+1最为假消息id
        public void Forward2Shamrock(string content)
        {
            LogSystem.Log($"向Shamrock发消息:{content}");
            Request? request = JsonConvert.DeserializeObject<Request>(content);
            
            //LogSystem.Log($"{request.action}");
            bool isNeedJoin2MessageList = request.action switch
            {
                Action.delete_msg=>true,
                //Action.get_msg=>true,
                Action.set_essence_msg=>true,
                Action.delete_essence_msg=>true,
                Action.send_private_msg=>true,
                Action.send_group_msg=>true,
                Action.send_msg=>true,
                Action.set_friend_add_request=>true,
                Action.set_group_add_request=>true,
                Action.group_touch=>true,
                _ => false
            };
            if (isNeedJoin2MessageList)
            {
                messages.Add(content);
                ForwardMessageList2Shamrock(request);
            }
            else
            {
                ConnectShamrock.Instance.SendMessage(content);
            }
        }
        public void ForwardMessageList2Shamrock(Request? request=null)
        {
            if (messages.Count <= 0) return;
            long currentTime = Utils.Utils.GetTimeStamp();
            if (currentTime > nextSendMessageTime)
            {
                long randomNumber = new Random().Next(5, 30);
                nextSendMessageTime = currentTime + randomNumber;
                LogSystem.Log($"下一次发消息在{randomNumber}秒后！",consoleColor:ConsoleColor.DarkMagenta);
                ConnectShamrock.Instance.SendMessage(messages[0]);
                messages.RemoveAt(0);
            }
            else if(request!=null)
            {
                string requestFakeReturn = request.action switch
                {
                    Action.send_private_msg => ConnectBotFramework.Instance.FakeReturnWithMessageID(request),
                    Action.send_group_msg => ConnectBotFramework.Instance.FakeReturnWithMessageID(request),
                    Action.send_msg => ConnectBotFramework.Instance.FakeReturnWithMessageID(request),
                    _ => ConnectBotFramework.Instance.FakeReturnWithVoid(request)
                };
                Forward2BotFrame(requestFakeReturn);
            }
        }
        public void Forward2Shamrock(byte[] bytes)
        {
            LogSystem.Log($"向Shamrock发消息:{bytes}");
            ConnectShamrock.Instance.SendMessage(bytes);
        }
        public void Forward2BotFrame(string content)
        {
            LogSystem.Log($"向BOT框架发消息:{content}");
            foreach (var item in ConnectBotFramework.Instance.clientLinks)
            {
                item.SendMessage(content);
            }
            foreach (var item in ConnectBotFramework.Instance.webSocketClientLinks)
            {
                item.SendMessage(content);
            }
        }
        public void Forward2BotFrame(byte[] bytes)
        {
            LogSystem.Log($"向BOT框架发消息:{bytes}");
            foreach (var item in ConnectBotFramework.Instance.clientLinks)
            {
                item.SendMessage(bytes);
            }
            foreach (var item in ConnectBotFramework.Instance.webSocketClientLinks)
            {
                item.SendMessage(bytes);
            }
        }
    }
}
