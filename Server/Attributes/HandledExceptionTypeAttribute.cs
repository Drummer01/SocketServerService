using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HandledExceptionTypeAttribute : Attribute
    {
        Type[] types;

        public HandledExceptionTypeAttribute(params Type[] types)
        {
            this.types = types;
        }

        public bool MethodSupportHandling(Type type)
        {
            return Array.IndexOf(types, types) > -1;
        }

        public bool MethodSupportHandling(params Type[] types)
        {
            foreach(Type type in types)
            {
                if (Array.IndexOf(this.types, type) > -1) continue;
                else return false;
            }
            return true;
        }
    }
}
