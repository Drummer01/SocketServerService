using SocketServer.Core;
using SocketServer.Ws;
using System;

namespace SocketServer.Modules
{
    public abstract class Module : ISendable
    {
        public int ID { get; set; }

        public abstract void execute(ref Response response);

        public void send(Response response)
        {
            throw new NotImplementedException();
        }
    }
}
