using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Sock.Ws
{
    public class Response
    {
        #region Public constants
        public const string ACTION_CHANNEL_UPDATE = "channel.update";
        public const string ACTION_CHANNEL_DISCONNECT = "channel.disconnect";
        #endregion

        public object exception { set; get; }

        public dynamic data { set; get; }

        public string action { set; get; }

        [JsonIgnore]
        public Type[] RequiredModules { set; get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
