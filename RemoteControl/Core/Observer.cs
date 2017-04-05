using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControl.Core
{
    internal class Observer : IObervable
    {
        public IWebSocketConnection Connection { get; set; }

        public void Notify(object o)
        {
            this.Connection.Send(o.ToString());
        }
    }
}
