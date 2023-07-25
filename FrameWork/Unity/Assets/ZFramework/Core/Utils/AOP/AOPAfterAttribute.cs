using System;

namespace AOP
{
    /// <summary>
    /// 用于标注需要获取返回值，和指出对应的处理函数（静态函数）
    /// </summary>
    [AttributeUsage( AttributeTargets.Method)]
    public class AOPAfterAttribute : Attribute
    {
        public string FullClassName;
        public string StaticMethodName;
        public string AssemblyName;

        /// <summary>
        /// 指定处理函数
        /// </summary>
        /// <param name="_fullClassName">介入方法所属类,类似 NameSpace.ClassName</param>
        /// <param name="_staticMethodName">介入方法名</param>
        /// <param name="_assemblyName">类所在程序集名</param>
        public AOPAfterAttribute(string _fullClassName, string _staticMethodName, string _assemblyName)
        {
            FullClassName = _fullClassName;
            StaticMethodName = _staticMethodName;
            AssemblyName = _assemblyName;
        }
    }
}