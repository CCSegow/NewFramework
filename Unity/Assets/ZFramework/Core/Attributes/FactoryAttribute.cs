using UnityEngine;
using System.Collections;
namespace ZFramework.Core {
    public class FactoryAttribute : CustomAttribue
    {
        public bool Poolable;
        public FactoryAttribute(bool poolable) {
            Poolable = poolable;
        }
    }

}
