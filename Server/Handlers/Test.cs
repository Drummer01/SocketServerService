using Server.Sock.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Sock.Handlers
{
    public class Test : Handler
    {
        public bool test(HandlerArgs args)
        {
            return true;
        }
    }
}
