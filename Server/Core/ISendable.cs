using Server.Sock.Ws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Sock.Core
{
    public interface ISendable
    {
        void send(Response response);
    }
}
