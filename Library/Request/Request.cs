namespace Shamrock_Forward.Library.Request
{
    using Data;
    using Newtonsoft.Json.Linq;

    public class Request
    {
        public Action action;
        //JToken __params;
        //public JToken _params 
        //{
        //    get => __params;
        //    set 
        //    { 
        //        __params = value;
        //        requestParam = value.ToObject<Params>();
        //        requestParams = value.ToObject<List<Params>>();
        //    }
        //}
        //public Params requestParam;
        //public List<Params> requestParams;
        public JToken _params;
        //public List<Params> _params = new();
        public object echo;
    }
}
