using UnityEngine;
using System.Collections;
namespace ZFramework {
    public class MonoSingleton<T> : MonoBehaviour where T : UnityEngine.Object
    {
        private static T _instance;
        public static T Ins {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }
    }

}
