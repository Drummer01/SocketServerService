using Fleck;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControl.Core
{
    public static class Notifier
    {
        private static List<IObervable> _listeners = new List<IObervable>();

        public static void RegisterListener(IWebSocketConnection conn)
        {
            _listeners.Add(new Observer()
            {
                Connection = conn
            });
        }

        public static void RemoveListener(IWebSocketConnection conn)
        {
            _listeners.RemoveAll(l => l.Connection.Equals(conn));
        }

        public static IObervable GetListener(IWebSocketConnection conn)
        {
            return _listeners.Where(l => l.Connection.Equals(conn)).First();
        }

        public static void Notify(object data)
        {
            foreach(IObervable observer in _listeners)
            {
                Notify(observer, data);
            }
        }

        public static void Notify(IObervable observer, object data)
        {
            observer.Notify(data);
        }

        public static void Notify(IObervable[] observers, object data)
        {
            foreach (IObervable observer in observers)
            {
                Notify(observer, data);
            }
        }
    }
}
