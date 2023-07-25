using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System;

namespace AOP
{
    /// <summary>
    ///  //通过 ContextAttribute，IContributeObjectSink 获取类的上下文环境
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AOPContextAttribute : ContextAttribute, IContributeObjectSink
    {
        

        public AOPContextAttribute():base("AOPContext") { 
        
        }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new AOPHandler(nextSink);
        }
    }
}