using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZFramework.Core {
    public class PoolableObjectFactory : CSingleton<PoolableObjectFactory>
    {
        PoolableObjectFactory() {
            _factories = new Dictionary<Type, IFactory>();
            var assembly = this.GetType().Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract)
                    continue;

                object[] objs = type.GetCustomAttributes(typeof(FactoryAttribute), false);
                if (objs.Length == 0)
                    continue;

                foreach (FactoryAttribute attr in objs)
                {
                    if (attr.Poolable) {
                        _factories.Add(type,Activator.CreateInstance(type) as IFactory);
                    }
                }
            }
        }

        Dictionary<Type, IFactory> _factories;

        public T CreateItem<T>() where T :class, IPoolableObject {
            var item = _factories.Get(typeof(T))?.CreateNew() ;
            if (item != null) {
                return item as T;
            }
            return null;
        }
    }

}
