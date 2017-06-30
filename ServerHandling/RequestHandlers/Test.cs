using SocketServer.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerHandling.RequestHandlers
{
    public class Test : RequestHandler
    {
        public void foo()
        {
            throw new ArgumentException();
        }
    }
}
