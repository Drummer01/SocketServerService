using Server.Sock.Core;
using Server.Sock.Ws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Modules
{
    abstract class Module : ISendable
    {
        public int ID { get; set; }

        public abstract void execute(ref Response response);

        public void send(Response response)
        {
            throw new NotImplementedException();
        }
    }
}
