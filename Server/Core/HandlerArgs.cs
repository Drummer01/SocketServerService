using SocketServer.Core;
using SocketServer.Ws;
using System;

namespace SocketServerw.Core
{
    public class HandlerArgs
    {
        public User Caller { get; set; }

        public Request Request { get; set; }

        public object this[int index, bool optional = false]
        {
            get
            {
                try
                {
                    return Request.Args[index];
                }
                catch (Exception)
                {
                    if(optional)
                    {
                        return null;
                    }
                    throw new ArgumentException("Invalid params");
                }
            }
        }

        public T get<T>(int index, bool optional = false) where T : IConvertible
        {
            return (T)Convert.ChangeType(this[index, optional], typeof(T));
        }

        public enum ArgumentNum
        {
            Zero, One, Two, Three, Four, Five, Six, Seve, Eight, Nine
        }
    }
}
