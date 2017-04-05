using Fleck;
using Newtonsoft.Json;
using Server.Sock.Core;
using Server.Sock.Handlers;
using Server.Sock.Repository;
using Server.Sock.Ws;
using SocketServer.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Sock
{
    public class Chat
    {
        #region Singleton
        private static object locker = new object();

        private static Chat instance;

        public static Chat getInstance()
        {
            lock(locker)
            {
                if (instance == null)
                {
                    instance = new Chat();
                }
                return instance;
            }
        }
        #endregion

        public UserRepository usersRepo { get; set; }

        public ChannelRepository chanelRepo { get; set; }

        private Chat()
        {
            this.usersRepo = new UserRepository();
            this.chanelRepo = new ChannelRepository();
            this.usersRepo.setKey("users");
            this.chanelRepo.setKey("channels");

            Register.getInstance()
                .set(this.usersRepo)
                .set(this.chanelRepo);

            ReloadChannels();
        }

        public void ReloadChannels()
        {
            try
            {
                foreach (DataAccess.channels row in DataAccess.channels.getAll())
                {
                    var channel = new Core.Channel()
                    {
                        Id = row.id,
                        HasPassword = row.is_locked,
                        MaxUsers = row.max_users,
                        Name = row.name,
                        Title = row.title
                    };
                    this.chanelRepo.add(channel);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void OnOpen(IWebSocketConnection conn)
        {
            conn.Send("hi");
            ServerDiagnostics.GetInstance().IncrementTotalClientsCount();
            usersRepo.add(new User(conn));
        }

        public void OnClose(IWebSocketConnection conn)
        {
            User user = this.usersRepo.GetUserByConnection(conn);
            this.usersRepo.remove(user);
        }

        public void OnMessage(IWebSocketConnection from, string message)
        {
            Debug.WriteLine(message);
            User user = this.usersRepo.GetUserByConnection(from);
            Request request = JsonConvert.DeserializeObject<Request>(message);
            Listener listener = Listener.getInstance();

            try
            {
                int id = DataAccess.users.checkAccessToken(request.AccessToken) ?? -1;

                if(id.Equals(-1) || ( !user.Id.Equals(-1) && !user.Id.Equals(id)))
                {
                    throw new ServerException("Permissions denied", ServerException.INVALID_ACCESS_TOKEN);
                }

                user.identify(id);
                listener.invoke(user, request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ExceptionsLogs.Add(e);
                Response response = new Response();
                response.action = request.Action;
                response.exception = e;
                string json = JsonConvert.SerializeObject(response);
                from.Send(json);
            }
        }
    }
}
