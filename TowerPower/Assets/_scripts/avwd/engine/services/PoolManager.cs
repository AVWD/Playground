using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AVWD.Engine.Services
{
    public class PoolManager
    {
        private static List<IPool> _poolList = new List<IPool>();
        public List<IPool> PoolList {
            get
            {
                return _poolList;
            }
            set
            {
                _poolList = value;
            }
        }
        public void add(IPool pool)
        {
            _poolList.Add(pool);
        }
        public void init()
        {
            for (int i = 0; i < _poolList.Count; i++)
                _poolList[i].Init();
        }
        public void clear()
        {
            for (int i = 0; i < _poolList.Count; i++)
                _poolList[i].Clear();
        }
    }
}
