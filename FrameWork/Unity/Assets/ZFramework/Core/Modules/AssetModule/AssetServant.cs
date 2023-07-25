using System;
using System.Collections;
using System.Threading.Tasks;

namespace ZFramework.Core
{
    /// <summary>
    /// 资源服务
    /// 通过它来获取资源，释放资源
    /// 可以每个场景可以创建多个资源服务，来专门获取资源。（它会帮忙去资源管理处获取资源并缓存，以便下次获取时）
    /// 在离开场景时只需要释放这这些服务，就可以释放通过这个服务获取的所有资源。
    /// 如果有些资源不想释放，可以创建一个常驻的服务
    /// </summary>
    public interface I_AssetServant
    {
        /// <summary>
        /// 同步获取资源
        /// </summary>
        /// <param name="assetUrl">资源路径</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        T GetAsset<T>(string assetUrl) where T : UnityEngine.Object;

        /// <summary>
        /// 异步获取资源（通过回调）
        /// </summary>
        /// <param name="assetURL"></param>
        /// <param name="onGet"></param>
        /// <typeparam name="T"></typeparam>
        void GetAssetAsync<T>(string assetURL, Action<T> onGet) where T : UnityEngine.Object;

        /// <summary>
        /// 异步获取资源
        /// </summary>
        /// <param name="assetURL"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetAssetAsyncTask<T>(string assetURL) where T : UnityEngine.Object;
        
        /// <summary>
        /// 异步获取原生资源，一次性，获取完会自动销毁资源，每次获取都是重新加载
        /// </summary>
        /// <param name="assetURL"></param>
        /// <returns></returns>
        Task<byte[]> GetRawAssetAsyncTask(string assetURL);

        byte[] GetRawAssetBytes(string assetURL);
        string GetRawAssetText(string assetURL);
        
        void LoadScene(string sceneURL,bool isAdditive,bool activeOnload,Action onBegin,Action onEnd,Action<float> onLoading);
        

        /// <summary>
        /// 释放指定资源
        /// </summary>
        /// <param name="assetURL"></param>
        void ReleaseAsset(string assetURL);
        
        
        /// <summary>
        /// 释放所有资源
        /// </summary>
        void DisposeAll();
    }




    
}