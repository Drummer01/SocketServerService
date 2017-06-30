using SocketServer.Ws;
using SocketServer.Handlers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SocketServer.Core
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

        private Dictionary<string, RequestHandler> map;

        private Listener()
        {
            map = new Dictionary<string, RequestHandler>();
        }

        public void invoke(User caller, Request request)
        {
            RequestHandler handler = getHandler(request.Handler);

            if( handler == null || !handler.MethodExists(request.Method) )
            {
                throw new ServerException("Invalid action", ServerException.INVALID_ACTION);
            }

            MethodInfo method = handler.GetType().GetMethod(request.Method);
            Executor exec = new Executor(caller, handler, method, request);

            Task.Factory.StartNew(exec.run);
        }

        public Listener registerHandler(RequestHandler handler)
        {
            map.Add(handler.Name.ToLower(), handler);
            GC.SuppressFinalize(handler);
            return this;
        }

        private RequestHandler getHandler(string handlerName)
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
            foreach(RequestHandler handler in this.map.Values)
            {
                GC.ReRegisterForFinalize(handler);
            }
            this.map.Clear();
        }
    }
}
