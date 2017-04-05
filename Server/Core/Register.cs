using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Sock.Core
{
    class Register
    {
        private static object locker = new object();

        private static Register instance;

        public static Register getInstance()
        {
            lock(locker)
            {
                if (instance == null)
                {
                    instance = new Register();
                }
                return instance;
            }
        }


        private Dictionary<object, IRegistreable> map;

        private Register()
        {
            map = new Dictionary<object, IRegistreable>();
        }

        public Register set(IRegistreable item)
        {
            if(!map.ContainsKey(item.getKey()))
            {
                map.Add(item.getKey(), item);
            }
            return this;
        }

        public IRegistreable get(object key)
        {
            if (map.ContainsKey(key)) return map[key];
            throw new KeyNotFoundException();
        }
    }
}
