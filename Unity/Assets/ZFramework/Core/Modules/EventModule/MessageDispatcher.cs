using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ZFramework {
    public class MessageDispatcher
    {

        static Dictionary<string, Action<object>> _registeredMessages = new Dictionary<string, Action<object>>();
        

        public static void Register(string msgName,Action<object> callBack)
        {
            if (!_registeredMessages.TryGetValue(msgName,out var targetEvent)) {
                targetEvent = _ => { };
                _registeredMessages.Add(msgName,targetEvent);                
            }
            targetEvent += callBack;
            _registeredMessages[msgName] = targetEvent;
        }
        public static void UnRegisterAll(string msgName)
        {
            if (_registeredMessages.ContainsKey(msgName))
            {
                _registeredMessages.Remove(msgName);
            }
        }
        public static void UnRegister(string msgName, Action<object> callBack) 
        {            
            if (_registeredMessages.TryGetValue(msgName, out var targetEvent)) {                
                targetEvent -= callBack;
                _registeredMessages[msgName] = targetEvent;

                if (targetEvent.GetInvocationList().Length == 0) {
                    _registeredMessages.Remove(msgName);
                }
            }            
        }

        public static void Send(string msgName,object msg)
        {
            if (_registeredMessages.TryGetValue(msgName, out Action<object> targetEvent)) {
                targetEvent.Invoke(msg);
            }
            // else {
            //     Debug.LogError($"No Msg name {msgName}");
            // }
        }

        public static void Dispose()
        {
            _registeredMessages.Clear();
        }
    }

}
