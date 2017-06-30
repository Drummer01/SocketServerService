using Server.Sock.Repository;
using Server.Sock.Ws;
using System.Runtime.Serialization;
using System.Linq;
using System.Threading;
using DataAccess;
using System;

namespace Server.Sock.Core
{
    [DataContract]
    public class Channel : ISendable, IDataUpdateable
    {
        [DataMember]
        public Message LastMessage;

        [DataMember]
        public bool HasPassword;

        [DataMember]
        public int Id, MaxUsers;

        [DataMember]
        public string Name;

        [DataMember]
        public int UsersCount
        {
            get
            {
                return this.userRepo.all().Count;
            }
        }

        public object UserList
        {
            get
            {
                return this.userRepo.all();
            }
        }


        private UserRepository userRepo;

        private ReaderWriterLock locker = new ReaderWriterLock();

        public Channel()
        {
            this.userRepo = new UserRepository();
        }

        public void attachUser(User user)
        {
            this.userRepo.add(user);
            user.Channel = this;
        }

        public void detachUser(User user)
        {
            this.userRepo.remove(user);
            user.Channel = null;
        }

        public bool kick(int id, Response reason)
        {
            return this.kick(this.userRepo.GetUserById(id), reason);
        }

        public bool kick(User user, Response reason)
        {
            if (user.Channel != this) return false;
            user.send(reason);
            this.detachUser(user);
            return true;
        }

        public bool kickAll(Response reason)
        {
            try
            {
                locker.AcquireReaderLock(2000);
                foreach (User user in this.userRepo)
                {
                    this.kick(user, reason);
                }
            }
            catch(System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
            finally
            {
                locker.ReleaseReaderLock();
            }
            return true;
        }

        public void send(Response response)
        {
            //foreach(ISendable item in this.userRepo)
            //{
            //    item.send(response);
            //}
            userRepo.send(response);
        }

        public void sendExceptOf(Response response, ISendable[] users)
        {
            foreach (ISendable item in this.userRepo)
            {
                if (users.Contains(item)) continue;
                item.send(response);
            }
        }

        [DataContract]
        public class Message
        {
            [DataMember]
            public int Sender;
            [DataMember]
            public int Time;
            [DataMember]
            public string Text;
        }

        public void Update()
        {
            using (DataModel dm = new DataModel())
            {
                channels data = dm.channels.Where(c => c.id == this.Id).First();

                Name        = data.name;
                HasPassword = data.is_locked;
            }
        }

        public static Channel Create(int id)
        {
            using (DataAccess.DataModel dm = new DataAccess.DataModel())
            {
                return Create(dm.channels.Where(c => c.id == id).FirstOrDefault());
            }
        }

        public static Channel Create(DataAccess.channels data)
        {
            DataAccess.messages lastMessageData = null;
            using (DataAccess.DataModel dm = new DataAccess.DataModel())
            {
                lastMessageData = dm.messages.Where(m => m.cid == data.id).OrderByDescending( m => m.time).FirstOrDefault();
            }
            return new Channel()
            {
                Id = data.id,
                HasPassword = data.is_locked,
                MaxUsers = 100,
                Name = data.name,
                LastMessage = new Channel.Message()
                {
                    Sender  = lastMessageData.cid,
                    Time    = lastMessageData.time,
                    Text    = lastMessageData.message    
                }
           
            };
        }
    }
}
