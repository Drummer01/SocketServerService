using Fleck;
using System;

namespace RemoteControl.Core
{
    public class Controller
    {
        #region Singleton
        private static Controller instance;

        public static Controller GetInstance()
        {
            if(instance == null)
            {
                instance = new Controller();
            }
            return instance;
        }

        private Controller()
        {
        }
        #endregion

        private Implementator impl;

        public void SetImplementator(Implementator impl)
        {
            this.impl = impl;
        }

        public void onMessage(IWebSocketConnection from, string message)
        {
            IObervable caller = Notifier.GetListener(from);

            System.Diagnostics.Debug.WriteLine(message);
            MulticastDelegate e = this.impl.GetEvent(message);
            System.Diagnostics.Debug.WriteLine(e);
            if (e == null) return;
            foreach (var handler in e.GetInvocationList())
            {
                handler.Method.Invoke(this.impl, new object[] { caller });
            }

        }
    }
}
