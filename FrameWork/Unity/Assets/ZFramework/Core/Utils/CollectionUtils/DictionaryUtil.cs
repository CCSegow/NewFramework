using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZFramework {
    public static class DictionaryUtil
    {
        public static V Get<K, V>(this Dictionary<K, V> dic, K key) {
            dic.TryGetValue(key, out V value);
            return value;
        }

        public static bool TryAdd<K,V>(this Dictionary<K,V> dic,K key,V value) {
            if (!dic.TryGetValue(key,out V data)) {
                dic.Add(key,value);
                return true;
            }
            return false;
        }
        
        public static void Add<K, V>(this Dictionary<K, V> dic, K key, V value,bool replaceExist)
        {
            if (!dic.TryGetValue(key, out V data))
            {
                dic.Add(key, value);
            }
            else {
                if (replaceExist)
                    dic[key] = value;
            }            
        }

        
        public static void Add<K, V , U>(this Dictionary<K, V> dic, K key, U value) where V : IList<U> ,new ()
        {
            if (!dic.TryGetValue(key, out V list))
            {
                list = new V();
                dic.Add(key, list);
            }
            list.Add(value);
        }
        public static void AddRange<K, V, U>(this Dictionary<K, V> dic, K key, IEnumerable<U> values) where V : List<U>, new() 
        {
            if (!dic.TryGetValue(key, out V list))
            {
                list = new V();
                dic.Add(key, list);
            }
            list.AddRange(values);
        }
        public static void Remove<K, V, U>(this Dictionary<K, V> dic, K key, U value) where V : List<U>, new()
        {
            if (dic.TryGetValue(key, out V list))
            {
                list.Remove(value);
            }
        }
        public static bool Contains<K, V, U>(this Dictionary<K, V> dic, K key, U value) where V : List<U>, new()
        {
            if (dic.TryGetValue(key, out V list))
            {
                return list.Contains(value);
            }
            else {
                return false;
            }
        }
    }

}
