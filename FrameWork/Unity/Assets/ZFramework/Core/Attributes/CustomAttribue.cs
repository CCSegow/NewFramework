using UnityEngine;
using System.Collections;
using System;

namespace ZFramework.Core {
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class CustomAttribue : Attribute
    {
        public Type AttributeType { get; private set; }

        public CustomAttribue() {
            AttributeType = this.GetType();
        }
    }

}
