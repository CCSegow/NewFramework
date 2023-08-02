using NUnit.Framework;
using UnityEngine;
using ZFramework.Utils.Encrypt;

namespace Tests
{
    public class EncryptTest
    {

        [Test]
        public void SingletonTestSimplePasses()
        {
            string result = EncryptUtil.Encrypt("Hello Yimi");

            Debug.Log(result);

            string input = EncryptUtil.Decrypt(result);
            Debug.Log(input);
        }

        
    }
}