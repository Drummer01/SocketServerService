using DataAccess;
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
        public Service()
        {
            InitializeComponent();
        }

        public void OnDebugStart()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            var allChans = channels.all();
            foreach(channels chan in allChans)
            {
                System.Diagnostics.Debug.WriteLine(chan.name);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
