using RemoteControl.Core;
using SocketServer;
using SocketServer.Core;
using Fleck;
using System.Diagnostics;
using System.Net.NetworkInformation;
using SocketServer.Helpers;

namespace SocketServerService
{
    internal static class ServerHandler
    {
        static Chat chat;
        static Listener listener;
        static WebSocketServer server;
        static string runningIp = "not specified";

        internal static void onStart(IObervable sender)
        {
            if (chat != null)
            {
                sender?.Notify("Server already started");
                return;
            }
            chat = Chat.getInstance();
            chat.LoadChannels();

            listener = Listener.getInstance();

            listener
                .registerHandler(new ServerHandling.RequestHandlers.Channel())
                .registerHandler(new ServerHandling.RequestHandlers.Message());

            runningIp = $"ws://{GetIP()}:2000";
            server = new WebSocketServer(runningIp);

            server.Start(socket =>
            {
                socket.OnOpen = () => chat.OnOpen(socket);
                socket.OnClose = () => chat.OnClose(socket);
                socket.OnMessage = message => chat.OnMessage(socket, message);
                socket.OnError = e => Debug.WriteLine(e);
            });

            sender?.Notify("Server started");
        }

        internal static void onStop(IObervable sender)
        {
            if (chat == null)
            {
                sender.Notify("Server already stopped");
                return;
            }
            chat = null;
            listener.Clear();
            listener = null;
            server.Dispose();
            sender.Notify("Server stopped");
        }

        internal static void onChannelsReload(IObervable sender)
        {
            if (chat == null)
            {
                sender.Notify("Server not started");
                return;
            }

            chat.LoadChannels();

            sender.Notify(string.Format("Channels loaded. Total loaded channels: {0}", chat.chanelRepo.all().Count));
        }

        internal static void onInfo(IObervable sender)
        {
            ServerDiagnostics diag = ServerDiagnostics.GetInstance();
            sender.Notify(string.Format("Running IP:{0}\nTotal:\nClients joins count: {1}\nChannels count: {2}", runningIp, diag.TotalClientsCount, Chat.getInstance().chanelRepo.all().Count));
        }

        internal static void onSaveExceptions(IObervable sender)
        {
            string path = ExceptionsLogs.SaveToFile();
            ExceptionsLogs.Clear();
            sender.Notify(string.Format("Exceptions saved succesfully. Log path: {0}", path));
        }

        internal static void onRequestServerState(IObervable sender)
        {
            int state = chat == null ? 0 : 1;
            sender.Notify(state);
        }

        private static object GetIP()
        {
            string strIp = string.Empty;
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var ips = ni.GetIPProperties().UnicastAddresses;
                    // return string.Join("|", ips.Select( x => x.Address.ToString()).ToList());
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            strIp = ip.Address.ToString();
                        }
                    }
                }
            }
            return strIp;
        }
    }
}
