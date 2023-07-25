using System;
using System.Reflection;

namespace AOP
{
    public static class ReflectionUtil
    {
        public static T GetAttribute<T>(MethodInfo method) where T : Attribute
        {
            //Console.WriteLine("尝试反射对象方法" + method.Name);
            var attrs = method.GetCustomAttributes(typeof(T), false);
            if (attrs.Length != 0)
            {
                var attribute = attrs[0] as T;
                if (attribute != null)
                {
                    return attribute;
                }
            }
            return null;
        }
    }
}
