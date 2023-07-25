using UnityEngine;
using System.Collections;
namespace ZFramework.Core {
    public abstract class BaseFactory<T> : IFactory
    {
        public abstract object CreateNew();
    }

}
