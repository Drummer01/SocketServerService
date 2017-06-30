using SocketServer.Handlers;
using SocketServer.Ws;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using SocketServer.Attributes;
using System.Diagnostics;
using System.Linq;

namespace SocketServer.Core
{
    [DataContract]
    public class ServerException : Exception
    {
        public const int INVALID_ACTION             = 1;
        public const int INVALID_ARGUMENTS          = 2;
        public const int INVALID_ACCESS_TOKEN       = 3;
        public const int ITEM_NOT_FOUND             = 4;

        public ServerException(string message, int code) : base(message)
        {
            this._code = code;
        }

        [DataMember]
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }

        private int _code;
        [DataMember]
        public int Code
        {
            get
            {
                return this._code;
            }
        }

        public static class Listener
        {
            private static object locker = new object();

            private static Dictionary<string, ExceptionHandler> map = new Dictionary<string, ExceptionHandler>();

            public static void registerHandler(ExceptionHandler handler)
            {
                map.Add(handler.HandledNamespace, handler);
                GC.SuppressFinalize(handler);
            }

            public static Exception handleException(Exception exception)
            {
                string handledNamespace = getHandledNamespace(exception);

                ExceptionHandler handler = getHandler(handledNamespace);
                MethodInfo[] methods = handler
                    .GetType()
                    .GetMethods()
                    .Where(m => m.GetCustomAttribute<HandledExceptionTypeAttribute>() != null)
                    .ToArray();

                foreach (MethodInfo method in methods)
                {
                    return (ServerException) method.Invoke(handler, new object[] { exception });
                }
                return exception;
            }

            public static void Clear()
            {
                foreach(ExceptionHandler handler in map.Values)
                {
                    GC.ReRegisterForFinalize(handler);
                }
                map.Clear();
            }

            private static string getHandledNamespace(Exception e)
            {
                string[] parts = e.GetType().ToString().Split('.');
                return parts[0];
            }

            private static ExceptionHandler getHandler(string @namespace)
            {
                try
                {
                    return map[@namespace];
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }
        }
    }
}
