using System;
using RemoteControl.Core;
using Server.Sock;
using Server.Sock.Core;
using Fleck;

namespace SocketServerService
{
    internal static class Server
    {
        static Chat chat;
        static Listener listener;
        static WebSocketServer server;
        static string runningIp = "not specified";

        internal static void onStart(IObervable sender)
        {
            if (chat != null)
            {
                sender.Notify("Server already started");
                return;
            }
            chat = Chat.getInstance();

            listener = Listener.getInstance();

            listener
                .registerHandler(new Server.Sock.Handlers.Channel())
                .registerHandler(new Message());

            runningIp = $"ws://{GetIP()}:2000";
            server = new WebSocketServer(runningIp);

            server.Start(socket =>
            {
                socket.OnOpen = () => chat.OnOpen(socket);
                socket.OnClose = () => chat.OnClose(socket);
                socket.OnMessage = message => chat.OnMessage(socket, message);
                socket.OnError = e => Debug.WriteLine(e);
            });

            sender.Notify("Server started");
        }

        internal static void onStop(IObervable sender)
        {

        }

        internal static void onChannelsReload(IObervable sender)
        {

        }

        internal static void onInfo(IObervable sender)
        {
            
        }
    }
}
