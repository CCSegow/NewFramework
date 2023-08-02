/*
 *  a simple class singleton
 */

using UnityEngine;
using System.Collections;
namespace ZFramework {
    public abstract class CSingleton<T> where T : class 
    {
        private static T _instance;
        public static T Ins {
            get {
                if (_instance == null) {
                    var type = typeof(T);
                    _instance =  System.Activator.CreateInstance(type,true) as T;

                }
                return _instance;
            }
        }
    }
}
