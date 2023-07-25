using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.Diagnostics;

namespace AOP
{
    //参考资料： https://blog.csdn.net/ls9512/article/details/78981285

    public class AOPHandler : IMessageSink
    {
        /// <summary>
        /// 下一个接收器
        /// </summary>
        private readonly IMessageSink _nextSink;

        public IMessageSink NextSink => _nextSink;

        public AOPHandler(IMessageSink nextSink) {
            _nextSink = nextSink;
        }
        /// <summary>
        /// 同步处理方法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage message = null;
            var callMessage = msg as IMethodCallMessage;
            if (callMessage != null)
            {
                // Before
                var before = ReflectionUtil.GetAttribute<AOPBeforeAttribute>(callMessage.MethodBase as MethodInfo);
                if (before != null)
                {
                    PreProceed(msg, before);
                }
                // Invoke
                message = _nextSink.SyncProcessMessage(msg);
                // After
                var after = ReflectionUtil.GetAttribute<AOPAfterAttribute>(callMessage.MethodBase as MethodInfo);
                if (after != null)
                {
                    PostProceed(message, after);
                }
            }
            else
            {
                message = _nextSink.SyncProcessMessage(msg);
            }
            return message;
        }
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }


        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="msg"></param>
        public void PreProceed(IMessage msg, AOPBeforeAttribute before)
        {
            var message = msg as IMethodMessage;
            Assembly assembly = Assembly.Load(string.Format("{0}, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", before.AssemblyName));
            string typeName = before.FullClassName;
            var type = assembly.GetType(typeName);
            UnityEngine.Debug.Log($"get type {typeName}, {type == null} ");
            //获取到的参数列表
            var param = message.Args;
            foreach (var item in param)
            {
                UnityEngine.Debug.Log($"参数 {item}");
            }
            
            type.InvokeMember(before.StaticMethodName, BindingFlags.InvokeMethod , null, null, param);
        }

        /// <summary>
        /// 方法执行后
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="after"></param>
        public void PostProceed(IMessage msg, AOPAfterAttribute after)
        {
            var message = msg as IMethodReturnMessage;            
            Assembly assembly = Assembly.Load(string.Format("{0}, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", after.AssemblyName));


            string typeName = after.FullClassName;
            var type = assembly.GetType(typeName);
            //获取到的返回值
            var param = message.ReturnValue;
            if (param == null)
            {
                UnityEngine.Debug.Log("retuen void");
                type.InvokeMember(after.StaticMethodName, BindingFlags.InvokeMethod, null, null, null);
            }
            else
            {
                type.InvokeMember(after.StaticMethodName, BindingFlags.InvokeMethod, null, null, new[] { param });
            }
        }
    }
}