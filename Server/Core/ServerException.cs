using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Core
{
    [DataContract]
    public class ServerException : Exception
    {
        private int _code;

        public const int INVALID_ACTION = 1;
        public const int INVALID_ARGUMENTS = 2;
        public const int INVALID_ACCESS_TOKEN = 3;

        public ServerException(string message, int code) : base(message)
        {
            this._code = code;
        }

        [DataMember]
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        [DataMember]
        public int Code
        {
            get
            {
                return this._code;
            }
        }

        public static ServerException createFrom(Exception ex)
        {
            ServerException se = new ServerException($"Unknown :> {ex.Message}", 0);
            if( ex is ArgumentException || 
                ex is InvalidCastException || 
                ex is FormatException ||
                ex is InvalidOperationException)
            {
                se = new ServerException("Invalid params", INVALID_ARGUMENTS);
            }

            return se;
        }
    }
}
