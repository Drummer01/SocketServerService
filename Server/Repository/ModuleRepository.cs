using Server.Sock.Core;
using SocketServer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Sock.Repository
{
    class ModuleRepository : Repository<Module>
    {
        public ModuleRepository get(Type type)
        {
            ModuleRepository newRepo = new ModuleRepository();
            foreach(var module in storage)
            {
                if(module.GetType().Equals(type))
                {
                    newRepo.add(module);
                }
            }
            return newRepo;
        } 
    }
}
