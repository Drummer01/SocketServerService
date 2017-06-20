using Fleck;

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
