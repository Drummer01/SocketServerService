using SocketServer.Attributes;
using SocketServer.Core;
using SocketServer.Handlers;
using SocketServer.Ws;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerHandling.ExceptionHandlers
{
    [HandledExceptionNamespaceAttribute("System")]
    public class MainHandler : ExceptionHandler
    {
        [HandledExceptionType(typeof(InvalidOperationException))]
        public ServerException handleItemNotFound(Exception exception)
        {
            return new ServerException(exception.Message, 1);
        }
    }
}
