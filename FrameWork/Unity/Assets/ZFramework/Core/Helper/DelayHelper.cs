using UnityEngine;
using System.Collections;
using System;
using UniRx;
namespace ZFramework {
    public class DelayHelper
    {
        public void Invoke(float time,Action callBack) {
            Observable.Timer(TimeSpan.FromSeconds(time))
                .Subscribe( _=> callBack());
        }
    }

}
