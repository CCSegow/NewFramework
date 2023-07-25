using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZFramework {
    public class ListPool<T>
    {
        // Fields
        private static Stack<List<T>> m_stack;

        // Methods
        static ListPool()
        {
            m_stack = new Stack<List<T>>();
        }
        public static void Add(List<T> _List)
        {
            _List.Clear();
            m_stack.Push(_List);
        }
        public static void Destroy()
        {
            m_stack.Clear();
        }
        public static List<T> Get()
        {
            if (m_stack.Count > 0)
            {
                return m_stack.Pop();
            }
            return new List<T>();
        }
    }

}
