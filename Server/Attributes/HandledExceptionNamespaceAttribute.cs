using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandledExceptionNamespaceAttribute : Attribute
    {
        public string Namespace { get; private set; }
        
        public HandledExceptionNamespaceAttribute(string value)
        {
            Namespace = value;
        }
    }
}
