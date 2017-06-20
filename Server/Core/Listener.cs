using Server.Sock.Handlers;
using Server.Sock.Ws;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Server.Sock.Core
{
    public class Listener
    {
        private static object locker = new object();

        private static Listener instance;

        public static Listener getInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new Listener();
                }
                return instance;
            }
        }

        private Dictionary<string, Handler> map;

        private Listener()
        {
            map = new Dictionary<string, Handler>();
        }

        public void invoke(User caller, Request request)
        {
            Handler handler = getHandler(request.Handler);

            if( handler == null || !handler.MethodExists(request.Method) )
            {
                throw new ServerException("Invalid action", ServerException.INVALID_ACTION);
            }

            MethodInfo method = handler.GetType().GetMethod(request.Method);
            Executor exec = new Executor(caller, handler, method, request);

            Task.Factory.StartNew(exec.run);
        }

        public Listener registerHandler(Handler handler)
        {
            map.Add(handler.Name.ToLower(), handler);
            GC.SuppressFinalize(handler);
            return this;
        }

        private Handler getHandler(string handlerName)
        {
            try
            {
                return map[handlerName];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public void Clear()
        {
            foreach(Handler handler in this.map.Values)
            {
                GC.ReRegisterForFinalize(handler);
            }
            this.map.Clear();
        }
    }
}
