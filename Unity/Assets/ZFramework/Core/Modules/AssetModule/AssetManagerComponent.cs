using System;
using System.Threading.Tasks;

namespace ZFramework.Core
{
    /// <summary>
    /// 资源管理，自定义的资源管理必须提供自己的资源服务
    /// </summary>
    public abstract class AssetManagerComponent: ZeroFrameWorkComponent
    {
        public bool IsReady { get; private set; }
        public override Type GetComponentType => typeof(AssetManagerComponent);

        //开始资源更新
        public abstract void BeginAssetUpdate(Action onFinish);

        /// <summary>
        /// 获取资源服务，可以通过它来获取资源和释放资源
        /// </summary>
        /// <returns></returns>
        public abstract I_AssetServant GetServant();
/*
        /// <summary>
        /// 异步获取资源（回调方式）
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        protected abstract void GetAssetAsync<T>(string assetPath, Action<T> callback) where T:UnityEngine.Object;
        
        /// <summary>
        /// 异步获取资源，Task模式
        /// </summary>
        /// <param name="assetPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected abstract Task<T> GetAssetAsync<T>(string assetPath) where T:UnityEngine.Object;

        /// <summary>
        /// 释放资源
        /// </summary>
        protected abstract void ReleaseAsset();
        
        */
    }

}