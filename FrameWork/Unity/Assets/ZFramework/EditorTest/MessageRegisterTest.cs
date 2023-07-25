using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ZFramework;
using ZFramework.Core;

namespace Tests
{
    public class MessageRegisterTest 
    {

        [Test]
        public void SingletonTestSimplePasses()
        {
            
            ClassMessageRegister register = new ClassMessageRegister();
            register.Register("Test", OnReceived);

            
            register.UnRegister("Test");
            MessageDispatcher.Send("Test", "hello");
            MessageDispatcher.Send("Test", "hello1");

            register.Dispose();
            MessageDispatcher.Send("Test","hello2");

            MessageDispatcher.Dispose();
        }

        void OnReceived(object msg) {
            Debug.Log($"recive msg :{msg}");
        }


    }
}