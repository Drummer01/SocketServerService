using Server.Sock.Core;
using System.Linq;

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
