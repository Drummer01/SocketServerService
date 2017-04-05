using Fleck;

namespace RemoteControl.Core
{
    public interface IObervable
    {
        IWebSocketConnection Connection { get; set; }
        /// <summary>
        /// Notifying all atached listeners
        /// </summary>
        /// <param name="o">object to send</param>
        void Notify(object o);
    }
}
