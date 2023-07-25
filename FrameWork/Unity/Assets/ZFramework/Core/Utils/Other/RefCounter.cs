using System.Diagnostics;
using ZFramework.Core;

namespace ZFramework 
{    
    public abstract class RefCounter<T>
    {
        protected T _object;
        public T Object => _object;
        private int _refCount;
        public int RefCount => _refCount;

        public void Retain() {
            if (_refCount == 0) {
                OnFirstRetain();
            }
            _refCount++;

        }
        /// <summary>
        /// 当引用数为0时返回true
        /// </summary>
        /// <returns></returns>
        public bool Release() {
            _refCount--;
            UnityEngine.Debug.Log($"{_object}: {_refCount}");
            if (_refCount == 0) {
                OnLastRelease();

                return true;
            }
            return false;
        }


        protected virtual void OnFirstRetain() { }
        protected virtual void OnLastRelease() { }

    }

}
