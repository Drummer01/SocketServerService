using DataAccess;
using Fleck;
using RemoteControl.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerService
{
    public partial class Service : ServiceBase
    {
        WebSocketServer localRemoteControlServer;

        public Service()
        {
            InitializeComponent();
        }

        public void OnDebugStart()
        {
            OnStart(null);
        }

        public void OnDebugStop()
        {
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            localRemoteControlServer = new WebSocketServer("ws://127.0.0.1:2000");
            Controller controller = Controller.GetInstance();
            Implementator impl = Implementator.GetInstance();
            controller.SetImplementator(impl);

            impl.onStart += ServerHandler.onStart;
            impl.onStop += ServerHandler.onStop;
            impl.onInfo += ServerHandler.onInfo;
            impl.onChannelsReload += ServerHandler.onChannelsReload;
            impl.onSaveExceptions += ServerHandler.onSaveExceptions;

            localRemoteControlServer.Start(s =>
            {
                s.OnOpen = () => { Notifier.RegisterListener(s); };
                s.OnClose = () => { Notifier.RemoveListener(s); };
                s.OnMessage = message => { controller.onMessage(s, message); };
            });

            ServerHandler.onStart(null);
        }

        protected override void OnStop()
        {
            localRemoteControlServer.Dispose();
        }
    }
}
