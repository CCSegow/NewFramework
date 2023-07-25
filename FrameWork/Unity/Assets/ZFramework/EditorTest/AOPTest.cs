using AOP;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SomeHttpRequest : AOPContext
    {

        [AOPBefore("Tests.SomeHttpRequest", "Befor", "EditorTest")]
        [AOPAfter("Tests.SomeHttpRequest", "After", "EditorTest")]
        public int TestMethod1(int a, int b)
        {

            Debug.Log($"正式处理 {a} + {b}");
            return a + b;
        }

        public static void Befor(ref int a, ref int b)
        {
            Debug.Log($"Befor {a} + {b}");
            a = 200;
            b = 100;
        }
        public static void After(int result)
        {
            Debug.Log($"After {result}");
        }
    }
    
    public class AOPTest
    {        
        [Test]
        public void AOPTestSimplePasses()
        {
            SomeHttpRequest request = new SomeHttpRequest();

            //Debug.Log(Assembly.GetExecutingAssembly().GetName());
            var res = request.TestMethod1(1,2);
            Debug.Log($"res  = {res}");
        }

    }
}
