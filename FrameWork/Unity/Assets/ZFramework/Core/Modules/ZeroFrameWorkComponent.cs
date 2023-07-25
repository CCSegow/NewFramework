using System;
using UnityEngine;
using System.Collections;

namespace ZFramework.Core
{
    public abstract class ZeroFrameWorkComponent : MonoBehaviour
    {
        public abstract Type GetComponentType { get; }

        public void Init()
        {
            OnInit();
        }

        protected abstract void OnInit();

    }
}

