using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketServer.Helpers
{
    public static class ExceptionsLogs
    {
        private static List<Exception> exceptions = new List<Exception>();

        public static void Add(Exception e)
        {
            exceptions.Add(e);
        }

        public static IEnumerable<Exception> Get(int start, int count)
        {
            return exceptions.Skip(Math.Max(0, exceptions.Count - (count + start)));
        }
    }
}
