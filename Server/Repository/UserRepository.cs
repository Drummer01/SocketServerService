using Server.Sock.Core;
using Fleck;
using System.Linq;
using Server.Sock.Ws;

namespace Server.Sock.Repository
{
    public class UserRepository : Repository<User>, ISendable
    {
        public User GetUserByConnection(IWebSocketConnection conn)
        {
            return this.storage.Where(user => user.Connection.Equals(conn)).FirstOrDefault();
        }

        public User GetUserById(int id)
        {
            return this.storage.Where(user => user.Id.Equals(id) && !user.Id.Equals(-1)).FirstOrDefault();
        }

        public void send(Response response)
        {
            foreach (ISendable user in this)
            {
                user.send(response);
            }
        }
    }
}
