using System;
using UnityEngine;

namespace ZFramework.Core
{
    
    public class SceneManagerComponent : ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(SceneManagerComponent);

        protected override void OnInit()
        {
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneURL">场景资源路径</param>
        /// <param name="isAdditive">增量式添加</param>
        public void LoadScene(string sceneURL, bool isAdditive)
        {
            var assetMgr = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
            var servant = assetMgr.GetServant();
            servant.LoadScene(sceneURL,isAdditive,true,OnBegin,OnEnd,OnLoading);

            servant.DisposeAll();
        }
        
        void OnBegin()
        {            
            MessageDispatcher.Send(GameEvents.LoadingEvent_OnBegin, null);
        }

        void OnEnd()
        {            
            MessageDispatcher.Send(GameEvents.LoadingEvent_OnEnd, null);
        }

        void OnLoading(float progress)
        {
            //Debug.Log($"Loading Scene :{progress}");
            float rate = Mathf.Clamp01(progress / 0.9f);
            MessageDispatcher.Send(GameEvents.LoadingEvent_OnProgressChange, rate);
        }
        
    }

}
