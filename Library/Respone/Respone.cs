namespace Shamrock_Forward.Library.Respone
{
    using Data;

    public class Respone
    {
        public Status status;
        public int retcode;
        public string msg = string.Empty;
        public string wording = string.Empty;
        public Data data = new();
        public object echo;
    }
    public enum Status
    {
        ok,
        failed
    }
}
