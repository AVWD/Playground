using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AVWD.Engine.Services
{
    public class Pool<T> : IPool where T : class
    {
        private const int DEFAULT_SIZE = 32;
        private Stack<T> _stack;
        private Func<Pool<T>, T> _factory;
        private int _initialCapacity;
        private int _fetchCount;
        private int _maxFetchCount;

        public Pool(Func<Pool<T>, T> factory, int size = DEFAULT_SIZE)
        {
            this._initialCapacity = size;
            this._stack = new Stack<T>(size);
            this._factory = factory;
        }

        public void Init()
        {
            this.Clear();
            for (int i = 0; i < _initialCapacity; i++)
            {
                _stack.Push(_factory(this));
            }
        }

        public void Clear()
        {
            _stack.Clear();
        }

        public T Fetch()
        {
            T retVal;
            if (_stack.Count > 0)
                retVal = _stack.Pop();
            else
                retVal = _factory(this);

            return retVal;
        }

        public void Release(T node)
        {
            _stack.Push(node);
        }
    }
}
