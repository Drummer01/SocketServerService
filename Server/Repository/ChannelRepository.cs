using SocketServer.Core;
using System.Linq;

namespace SocketServer.Repository
{
    public class ChannelRepository : Repository<Channel>
    {
        public Channel GetChannelById(int id)
        {
            return storage.First(item => item.Id.Equals(id));
        }
    }
}
