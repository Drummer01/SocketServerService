using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer.Helpers
{
    public class ServerDiagnostics
    {
        #region Singleton
        private static ServerDiagnostics instance;

        public static ServerDiagnostics GetInstance()
        {
            if(instance == null)
            {
                instance = new ServerDiagnostics(); 
            }
            return instance;
        }

        private ServerDiagnostics()
        { }
        #endregion

        #region Constants and properties
        internal const int DEFAULT_LOCKER_TIMEOUT = 1000;
        ReaderWriterLock locker = new ReaderWriterLock();
        #endregion

        #region Public Properties and backing fields
        internal int _TotalClientsCount = 0;
        public int TotalClientsCount
        {
            get { return _TotalClientsCount; }
        }

        public int TotalRequestsCount
        {
            get { return _TotalHitRequests + _TotalMissRequests; }
        }

        internal int _TotalHitRequests = 0;
        public int TotalHitRequests
        {
            get { return _TotalHitRequests; }
        }

        internal int _TotalMissRequests = 0;
        public int TotalMissRequests
        {
            get { return _TotalMissRequests; }
        }
        #endregion

        #region Internal Methods for incrementing the counters
        internal void IncrementTotalClientsCount()
        {
            Interlocked.Increment(ref _TotalClientsCount);
        }

        internal void IncrementTotalHitRequests()
        {
            Interlocked.Increment(ref _TotalHitRequests);
        }

        internal void IncrementTotalMissRequests()
        {
            Interlocked.Increment(ref _TotalMissRequests);
        }
        #endregion
    }
}
