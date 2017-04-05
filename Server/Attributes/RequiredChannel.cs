using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiredChannel : Attribute
    {
        public bool required { get; set; }

        public RequiredChannel(bool required = true)
        {
            this.required = required;
        }
    }
}
