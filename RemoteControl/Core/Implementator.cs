using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControl.Core
{
    public class Implementator
    {
        private static Implementator instance;

        public static Implementator GetInstance()
        {
            if(instance == null)
            {
                instance = new Implementator();
            }
            return instance;
        }

        private Implementator()
        {
        }

        public MulticastDelegate GetEvent(string name)
        {
            return (MulticastDelegate)this
                .GetType()
                .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase)
                .GetValue(this);
        }

        public delegate void EventHandler(IObervable sender);

        public event EventHandler onStart;
        public event EventHandler onStop;
        public event EventHandler onInfo;
        public event EventHandler onChannelsReload;
        public event EventHandler onSaveExceptions;
        public event EventHandler onRequestServerState;
    }
}
