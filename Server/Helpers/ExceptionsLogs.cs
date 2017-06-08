using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SocketServer.Helpers
{
    public static class ExceptionsLogs
    {
        private const string LOGS_PATH = @"D:\";

        private static List<TimestampException> exceptions = new List<TimestampException>();

        public static void Add(Exception e)
        {
            exceptions.Add(new TimestampException() { Exception = e });
        }

        public static IEnumerable<Exception> Get(int start, int count)
        {
            return exceptions.Skip(Math.Max(0, exceptions.Count - (count + start))).Select( te => te.Exception );
        }

        public static string SaveToFile()
        {
            string fileName = LOGS_PATH + "SocketServerService_logs_" + DateTime.Now.ToString().Replace(" ", "_").Replace(":", "_") + ".log";
            Debug.WriteLine(fileName);
            System.IO.File.WriteAllLines(fileName, exceptions.Select( e => formatException(e) ).ToArray());
            return fileName;
        }

        private static string formatException(TimestampException e)
        {
            return string.Format("{0} | {1}", e.Time, e.Exception.Message);
        }

        private class TimestampException
        {
            internal Exception Exception { get; set; }
            internal DateTime Time = DateTime.Now;
        }

        public static void Clear()
        {
            exceptions.Clear();
        }
    }
}
