using System.Linq;

namespace SocketServer.Ws
{
    public class Request
    {
        private string _action;
        public string Action
        {
            set
            {
                string[] parts = value.Split('.');
                Handler = parts[0];
                Method = parts[1];
                _action = value;
            }
            get
            {
                return _action;
            }
            
        }

        public string AccessToken { set; get; }

        public string Handler { private set; get; }

        public string Method { private set; get; }

        public object[] Args { set; get; }

        public override string ToString()
        {
            return $" {AccessToken} | {Handler} {Method} {string.Join(",", Args.Select(x => x.ToString()).ToArray())} ";
        }
    }
}
