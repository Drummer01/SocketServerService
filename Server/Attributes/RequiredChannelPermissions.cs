using SocketServer.Core;
using SocketServerw.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequiredChannelPermissions : Attribute
    {
        public RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel level)
        {
            this.level = level;
        }

        public RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel level, HandlerArgs.ArgumentNum ArgNum)
        {
            this.level = level;
            this.ArgNum = ArgNum;
        }

        public DataAccess.ChannelPermissionsLevel level { get; set; }
        public HandlerArgs.ArgumentNum? ArgNum { get; set; }

    }
}
