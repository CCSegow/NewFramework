using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ZFramework.Core
{
    public class GameEntryComponent: ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(GameEntryComponent);
        protected override void OnInit()
        {
            
        }

        [LabelText("启动游戏的预制体"),FilePath]
        public string LaunchObjectAssetPath;
        
        // 开始游戏
        public async void Launch()
        {
            var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
            var servant = assetComponent.GetServant();            

            var launcherObj = GameObject.Instantiate( await servant.GetAssetAsyncTask<GameObject>(LaunchObjectAssetPath));
            servant.DisposeAll();
        }
        
    }
}