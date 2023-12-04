using Microsoft.Win32.SafeHandles;
using Shamrock_Forward.Instacne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shamrock_Forward
{
    internal class Config:InstanceAny<Config>
    {
        public string accessToken = "7HZXFz4F0VGGqWiWo4z";
        public int messageLength = 1024 * 1024 * 50;
        public long masterQQ = 1758158706;
        public long botQQ = 3399672731;
        public string shamrockIP = "192.168.137.88";
        public string shamrockPort = "12585";
        public string shamrockEndPoint=string.Empty;
        public string wsServerIP = "0.0.0.0";
        public string wsServerPort = "12585";
        public List<ws_rev> ws_Revs = new();
    }
    public class ws_rev
    {
        public string ws_revIP=string.Empty;
        public string ws_revPort=string.Empty;
        public string ws_revEndPoint=string.Empty;
    }
}
