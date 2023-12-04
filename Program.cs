using Shamrock_Forward.Log;
using System.Net.Sockets;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using Fleck;
using Newtonsoft.Json;

namespace Shamrock_Forward
{
    internal class Program
    {
        static void Main(string[] args)
        {
             new MainActivaty().Main(args);
        }
    }
    class MainActivaty
    {
        public void Main(string[] args)
        {
            string filePath = $@"{Directory.GetCurrentDirectory()}/Config.json";
            if (File.Exists(filePath))
            {
                Config.SetInstance(JsonConvert.DeserializeObject<Config>(File.ReadAllText(filePath)));
            }
            else
            {
                Config.Instance.ws_Revs.Add(new() { ws_revIP = "192.168.137.4", ws_revPort = "8998" });
                File.WriteAllText(filePath,JsonConvert.SerializeObject(Config.Instance,Formatting.Indented));
                LogSystem.Log(@$"你似乎没有配置文件...已经生成一个默认的配置文件在{filePath}，去修改配置后重新启动本程序");
                LogSystem.Log(@$"按任意键继续...");
                Console.ReadKey();
                return;
            }
            Thread runBotFramework = new(() => 
            {
                ConnectBotFramework.Instance.Init(Config.Instance.wsServerIP, Config.Instance.wsServerPort);
            });
            Thread runShamrock = new(() => 
            {
                ConnectShamrock.Instance.Init($@"ws://{Config.Instance.shamrockIP}:{Config.Instance.shamrockPort}{Config.Instance.shamrockEndPoint}");
            });
            runShamrock.IsBackground = true;
            runBotFramework.IsBackground = true;
            runShamrock.Start();
            runBotFramework.Start();
            while (true)
            {
                Console.ReadKey();
            }
        }
    }
}