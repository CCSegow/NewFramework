using UnityEngine;
using System.Collections.Generic;

namespace ZFramework.Core
{
    public enum E_EnvironmentMode
    {
        Developing,
        Test,
        Release,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Ins;

        public E_EnvironmentMode Mode;

        public URLConfigs URLConfigs;

        Dictionary<string, ZeroFrameWorkComponent> _components;

        private void Awake()
        {
            Ins = this;

            _components = new Dictionary<string, ZeroFrameWorkComponent>();
            var components = GetComponentsInChildren<ZeroFrameWorkComponent>();
            AddGameComponents(components);

            DontDestroyOnLoad(this);
        }

        void Start()
        {
            //初始化资源管理类
            var assetComponent = GetGameComponent<AssetManagerComponent>();
            assetComponent.Init();
                        
            //开始更新游戏资源
            var gameEntryComponent = GetGameComponent<GameEntryComponent>();
            assetComponent.BeginAssetUpdate(() =>
            {
                var hotfixComponent = GetGameComponent<HotFixComponent>();
                hotfixComponent.BeginLoadHotfix(() =>
                {
                    gameEntryComponent.Launch();    
                });                
            });
        }        

        void AddGameComponents(ZeroFrameWorkComponent[] components)
        {
            foreach (var item in components)
            {
                var type = item.GetComponentType;
                Debug.Log(type.Name);
                _components.Add(type.Name, item);
            }
        }

        public T GetGameComponent<T>() where T : ZeroFrameWorkComponent
        {
            var typeName = typeof(T).Name;
            //Debug.Log($"GetGameComponent {typeName}");
            if (!_components.TryGetValue(typeName, out var component))
            {
                Debug.LogError($"找不到组件 {typeName}");
            }

            return component as T;
        }
    }
}