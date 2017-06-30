using Fleck;
using SocketServer.Repository;
using SocketServer.Ws;
using SocketServer.Modules;
using System.Runtime.Serialization;

namespace SocketServer.Core
{
    [DataContract]
    public class User : ISendable
    {
        public IWebSocketConnection Connection { private set; get; }

        [DataMember]
        public int Id { private set; get; } = -1;
       
        public Channel Channel { set; get; }

        private ModuleRepository mRepo;

        public User(IWebSocketConnection conn)
        {
            this.Connection = conn;
            this.mRepo = new ModuleRepository();
        }

        public void identify(int id)
        {
            if(!this.Id.Equals(id) && id != -1)
            {
                this.Id = id;
                this.mRepo.add(Filter.create(this.Id));
            }
        }

        public void send(Response response)
        {
            if(response.RequiredModules != null)
            {
                foreach(var t in response.RequiredModules)
                {
                    ModuleRepository tRepo = mRepo.get(t);
                    foreach(var module in tRepo)
                    {
                        module.execute(ref response);
                    }
                }
            }
            this.Connection.Send(response.ToString());
        }
    }
}
