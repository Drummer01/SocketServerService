using SocketServer.Modules;
using System;


namespace SocketServer.Repository
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
