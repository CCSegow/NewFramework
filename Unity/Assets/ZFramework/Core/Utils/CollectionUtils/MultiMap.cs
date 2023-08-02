using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZFramework {
    public class MultiMap<K, V>
    {
        private readonly Dictionary<K, List<V>> _dic = new Dictionary<K, List<V>>();

        public void Add(K key, V value) {
            _dic.Add(key, value);
        }
        public void Remove(K key, V value) {
            _dic.Remove(key, value);
        }
        public void Remove(K key)
        {
            _dic.Remove(key);
        }
        public KeyValuePair<K, List<V>> First() {
            return _dic.First();
        }
        public int Count => _dic.Count;

        public bool Contains(K key, V value) {
            return _dic.Contains(key, value);
        }
        public bool ContainKey(K key) {
            return _dic.ContainsKey(key);
        }
        public void Clear() {
            _dic.Clear();
        }

        public V[] GetAll(K key){
            if (!_dic.TryGetValue(key ,out List<V> list)) {
                return null;
            }
            return list.ToArray();
        }
    }

}
