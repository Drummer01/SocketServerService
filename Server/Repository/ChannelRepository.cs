using Server.Sock.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Sock.Repository
{
    public class ChannelRepository : Repository<Channel>
    {
        public Channel GetChannelById(int id)
        {
            return storage.Where(item => item.Id.Equals(id)).FirstOrDefault();
        }
    }
}
