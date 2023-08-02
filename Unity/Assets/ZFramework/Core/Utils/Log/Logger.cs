using UnityEngine;
using System.Collections;

namespace ZFramework
{
    public class ZLogger
    {
        
        public static void Log(string text) {
            UnityEngine.Debug.Log(text);
        }

        public static void Warning(string text)
        {
            UnityEngine.Debug.LogWarning(text);
        }
        public static void Error(string text)
        {
            UnityEngine.Debug.LogError(text);
        }
    }
}