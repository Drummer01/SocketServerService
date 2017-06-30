using Server.Sock.Core;
using Server.Sock.Repository;
using Server.Sock.Ws;
using SocketServer.Attributes;
using System;

namespace Server.Sock.Handlers
{
    public class Channel : Handler
    {
        private static string md5Builder(System.Security.Cryptography.MD5 md5, string str)
        {
            byte[] bytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            string hash = string.Empty;

            for (int i = 0; i < bytes.Length; i++)
            {
                hash += bytes[i].ToString("x2");
            }
            return hash;
        }

        private static void notifyClients(string action, dynamic data)
        {
            Response res = new Response()
            {
                action = action + Response.ACTION_DONE,
                data = data
            };
            ISendable users = Register.getInstance().get("users") as ISendable;
            users.send(res);
        }

        [RequiredChannel(false)]
        public bool join(HandlerArgs args)
        {
            int id = args.get<int>(0);
            ChannelRepository repo = Register.getInstance().get("channels") as ChannelRepository;
            Core.Channel channel = repo.GetChannelById(id);
            if (channel == null) return false;

            if (channel.UsersCount >= 100) throw new Exception("too many clients");

            if(DataAccess.channel_bans.isBanned(args.Caller.Id, channel.Id))
            {
                throw new Exception("banned from channel");
            }
            
            if(channel.HasPassword)
            {

                //using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                //{
                //    string md5pass = md5Builder(md5, (string)args[1]);
                //    using (var context = new DataAccess.DataModel())
                //    {
                //        var data = context.channels.Where(item => item.id == channel.Id).Select(o => new { o.password }).First();
                //        if (data.password.Equals(md5pass))
                //        {
                //            channel.attachUser(args.Caller);
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    }
                //}

                string password = (string)args[1];
                if (password.Equals(password))
                {
                    channel.attachUser(args.Caller);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            channel.attachUser(args.Caller);
            return true;
        }

        [RequiredChannel]
        public bool leave(HandlerArgs args)
        {
            args.Caller.Channel.detachUser(args.Caller);
            return true;
        }

        [RequiredChannel]
        [RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel.Moderator)]
        public bool kick(HandlerArgs args)
        {
            int id = args.get<int>(0);
            string reason = (string)args[1];

            Response response = new Response()
            {
                action = Response.ACTION_CHANNEL_DISCONNECT + Response.ACTION_DONE,
                data = new
                {
                    reason = reason
                }
            };

            return args.Caller.Channel.kick(id, response);
        }

        [RequiredChannel]
        [RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel.Administrator)]
        public bool ban(HandlerArgs args)
        {
            DataAccess.channel_bans.saveBan(args.Caller.Channel.Id, args.get<int>(0), args.Caller.Id, args.get<string>(1), args.get<int>(2));
            return kick(args);
        }

        [RequiredChannel]
        [RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel.Administrator)]
        public bool unban(HandlerArgs args)
        {
            int uid = args.get<int>(0);

            DataAccess.channel_bans.removeBan(args.Caller.Channel.Id, uid);
            return true;
        }

        public object getlist(HandlerArgs args)
        {
            ChannelRepository repo = Register.getInstance().get("channels") as ChannelRepository;
            return repo.all();
        }

        [RequiredChannel]
        public object getlistusers(HandlerArgs args)
        {
            return args.Caller.Channel.UserList;
        }

        public bool create(HandlerArgs args)
        {
            string name = args.get<string>(0);
            string title = args.get<string>(1, true);
            string password = args.get<string>(2, true);

            int created_chann_id = DataAccess.channels.createNew(args.Caller.Id, name, title, password);

            if (created_chann_id.Equals(-1)) return false;

            ChannelRepository repo = Register.getInstance().get("channels") as ChannelRepository;
            Core.Channel chan = new Core.Channel()
            {
                Id = created_chann_id,
                HasPassword = (password != null),
                MaxUsers = 100,
                Name = name
            };
            repo.add(chan);

            notifyClients(Response.ACTION_CHANNEL_UPDATE + Response.ACTION_DONE, repo.all());
            return true;
        }

        [RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel.Administrator, HandlerArgs.ArgumentNum.Zero)]
        public bool remove(HandlerArgs args)
        {
            int id = args.get<int>(0);
            ChannelRepository repo = Register.getInstance().get("channels") as ChannelRepository;
            Core.Channel chan = repo.GetChannelById(id);

            bool success = DataAccess.channels.remove(id);
            if(success)
            {
                Response response = new Response()
                {
                    action = Response.ACTION_CHANNEL_DISCONNECT + Response.ACTION_DONE,
                    data = new
                    {
                        reason = "removing channel"
                    }
                };

                chan.kickAll(response);
                repo.remove(chan);

                notifyClients(Response.ACTION_CHANNEL_UPDATE + Response.ACTION_DONE, repo.all());
            }
            return success;
        }

        [RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel.Administrator, HandlerArgs.ArgumentNum.Zero)]
        public bool restore(HandlerArgs args)
        {
            int id = args.get<int>(0);
            bool success = DataAccess.channels.restore(id);
            if(success)
            {
                ChannelRepository repo = Register.getInstance().get("channels") as ChannelRepository;
                var row = DataAccess.channels.get(id);
                var chan = new Core.Channel()
                {
                    Id = row.id,
                    HasPassword = row.is_locked,
                    MaxUsers = row.max_users,
                    Name = row.name
                };
                repo.add(chan);

                notifyClients(Response.ACTION_CHANNEL_UPDATE + Response.ACTION_DONE, repo.all());
            }
            return success;
        }

        [RequiredChannel]
        [RequiredChannelPermissions(DataAccess.ChannelPermissionsLevel.Administrator)]
        public bool update(HandlerArgs args)
        {
            string newName = args.get<string>(0, true);
            string newTitle = args.get<string>(1, true);
            string newPassword = args.get<string>(2, true);
            bool hasPassword = (newPassword != null);

            var rawChan = DataAccess.channels.get(args.Caller.Channel.Id);

            Core.Channel chan = args.Caller.Channel;
            chan.Name = newName ?? chan.Name;

            //TODO: updating channel

            return true;
        }
    }
}