using System;
using UnityEngine;
using System.Collections;

namespace ZFramework.Core
{
    public abstract class HotFixComponent : ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(HotFixComponent);

        protected override void OnInit()
        {
            
        }

        /// <summary>
        /// 开始加载热更代码
        /// </summary>
        /// <param name="onFinish"></param>
        public abstract void BeginLoadHotfix(Action onFinish);

    }
}