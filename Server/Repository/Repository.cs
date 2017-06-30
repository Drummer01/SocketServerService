using SocketServer.Core;
using System;
using System.Collections.Generic;
using System.Collections;

namespace SocketServer.Repository
{
    public abstract class Repository<T> : IRegistreable, IEnumerable<T> where T : ISendable
    {
        protected IList<T> storage = new List<T>();

        private object key;

        public virtual void add(T item)
        {
            lock(storage)
            {
                this.storage.Add(item);
            }

        }

        public virtual void remove(T item)
        {
            lock(storage)
            {
                this.storage.Remove(item);
            }
        }

        public virtual void clear()
        {
            this.storage.Clear();
        }

        public IList<T> all()
        {
            lock (storage)
            {
                return this.storage;
            }
        }

        public object getKey()
        {
            return key;
        }

        public void setKey(object key)
        {
            this.key = key;
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (storage)
            {
                return this.storage.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
