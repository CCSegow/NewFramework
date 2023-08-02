using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ZFramework.Core;

namespace ZFramework {
    public interface IPoolableObject { 
        
    }
    public class PoolUtil<T> where T : class, IPoolableObject
    {
        private static Dictionary<Type, Queue<T>> _pools;
        private static Dictionary<Type, List<T>> _activeLists;

        static PoolUtil() {
            _pools = new Dictionary<Type, Queue<T>>();
            _activeLists = new Dictionary<Type, List<T>>();
        }


        public static T Get() {
            T item;

            var type = typeof(T);
            if (_pools.TryGetValue(type, out Queue<T> pool) && pool.Count > 0)
            {
                item = pool.Dequeue();
            }
            else {
                item = PoolableObjectFactory.Ins.CreateItem<T>();
            }

            if (!_activeLists.TryGetValue(type, out List<T> list))
            {
                list = new List<T>();
                _activeLists.Add(type,list);
            }
            list.Add(item);


            return item;
        }
        public static void Cache(T item) {
            var type = typeof(T);

            if (_activeLists.TryGetValue(type, out List<T> list))
            {
                list.Remove(item);
                if (list.Count == 0) {
                    _activeLists.Remove(type);
                }
            }

            if (!_pools.TryGetValue(type, out Queue<T> pool))
            {
                pool = new Queue<T>();
                _pools.Add(type,pool);
            }
            pool.Enqueue(item);
        }

        public static void ClearPool()
        {
            var type = typeof(T);
            _pools.Remove(type);
            _activeLists.Remove(type);
        }

        public static void ClearAllPool()
        {
            _pools.Clear();
            _activeLists.Clear();
        }

        public int PoolCount => _pools.Count;
        public int CacheCount() {
            var type = typeof(T);
            if (_pools.TryGetValue(type, out Queue<T> pool))
            {
                return pool.Count;
            }
            return 0;
        }
        public int ActiveCount() {
            var type = typeof(T);
            if (_activeLists.TryGetValue(type, out List<T> list))
            {
                return list.Count;
            }
            return 0;
        }
    }

}
