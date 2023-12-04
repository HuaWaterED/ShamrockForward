using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shamrock_Forward.Library.Event
{
    public class Heartbeat
    {
        public Heartbeat(long time) 
        {
            this.time = time;
        }
        public long time;
        public long self_id=Config.Instance.botQQ;
        public string post_type = "meta_event";
        public string meta_event_type = "lifecycle";
        public string sub_type = "connect";
        public Status status = new();
        public long interval =15000;
    }

    public class Status
    {
        public Self self = new();
        public bool online=true;
        public bool good=true;
        public string qqstatus = "正常";
    }

    public class Self
    {
        public string platform = "qq";
        public long user_id= Config.Instance.botQQ;
    }
}
