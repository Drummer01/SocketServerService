using Newtonsoft.Json;
using System;

namespace SocketServer.Ws
{
    public class Response
    {
        #region Public constants
        public const string ACTION_CHANNEL_DATA_UPDATE = "channel.updatedata";
        public const string ACTION_CHANNEL_DISCONNECT = "channel.disconnect";
        public const string ACTION_DONE = "+done";
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
