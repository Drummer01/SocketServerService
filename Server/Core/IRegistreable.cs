using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Core
{
    public interface IRegistreable
    {
        object getKey();
        void setKey(object key);
    }
}
