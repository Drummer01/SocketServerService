using Server.Sock.Core;
using Server.Sock.Ws;
using SocketServer.Attributes;
using SocketServer.Helpers;
using SocketServer.Modules;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace Server.Sock.Handlers
{
    public class Message : Handler
    {
        [RequiredChannel]
        public object send(HandlerArgs args)
        {
            string message = (string) args[0];
            string type = (string)args[1];
            Core.Channel channel = args.Caller.Channel;

            Newtonsoft.Json.Linq.JArray atachments = (Newtonsoft.Json.Linq.JArray)args[2, true];

            int messageId = saveMessage(args.Caller.Id, channel.Id, type, message);
            if(atachments != null)
            {
                saveAtachments(messageId, atachments.ToObject<string[]>());
            }

            Response res = new Response();
            dynamic msgData = new ExpandoObject();

            msgData.sender = args.Caller.Id;
            msgData.message = message;
            msgData.type = type;
            msgData.atachments = atachments;

            res.data = msgData;
            res.action = args.Request.Action + Response.ACTION_DONE;
            res.RequiredModules = new Type[] { typeof(Filter) };

            channel.send(res);

            //channel.sendExceptOf(res, new User[] { args.Caller });

            return true;
        }

        private int saveMessage(int uid, int cid, string type, string message)
        {
            DataAccess.messages messageModel = new DataAccess.messages();
            messageModel.uid        = uid;
            messageModel.cid        = cid;
            messageModel.time       = Util.UnixTimestamp();
            messageModel.type       = type;
            messageModel.message    = message;

            using (DataAccess.DataModel dm = new DataAccess.DataModel())
            {
                dm.messages.Add(messageModel);
                dm.SaveChanges();
            }

            return messageModel.id;
        }

        private void saveAtachments(int mid, string[] atachments)
        {
            using (DataAccess.DataModel dm = new DataAccess.DataModel())
            {
                foreach(var atachment in atachments)
                {
                    DataAccess.message_atachments atachmentModel = new DataAccess.message_atachments();
                    atachmentModel.mid = mid;
                    atachmentModel.link = atachment;
                    atachmentModel.type = "image/jpg";
                    atachmentModel.hidden = false;

                    dm.message_atachments.Add(atachmentModel);
                }
                dm.SaveChanges();
            }
        }
    }
}
