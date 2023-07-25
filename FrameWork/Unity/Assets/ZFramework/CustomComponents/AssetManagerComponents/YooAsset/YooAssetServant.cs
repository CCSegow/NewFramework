using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using ZFramework.Core;


public class YooAssetServant:I_AssetServant
{
    private HashSet<string> _loadedAssets;
    private YooAssetManagerComponent _AssetMgr;
    
    public YooAssetServant(YooAssetManagerComponent assetMgr)
    {
        _AssetMgr = assetMgr;
        _loadedAssets = new HashSet<string>();
    }

    #region 接口

    public T GetAsset<T>(string assetUrl) where T : UnityEngine.Object
    {
        _loadedAssets.Add(assetUrl);
        var obj = GetAssetSync(assetUrl);
        if (obj is GameObject) {
            if (typeof(T) != typeof(GameObject)) {
                return (obj as GameObject).GetComponent<T>();
            }
        }
        return obj as T;
    }   

    public void GetAssetAsync<T>(string assetUrl, Action<T> onGet) where T : UnityEngine.Object
    {
        _loadedAssets.Add(assetUrl);
        GetAssetAsync(assetUrl, (obj) =>
        {
            onGet(obj as T);
        });      
    }

    public async Task<T> GetAssetAsyncTask<T>(string assetURL) where T : UnityEngine.Object
    {
        _loadedAssets.Add(assetURL);
        return await GetAssetTask(assetURL) as T;
    }

    public async Task<byte[]> GetRawAssetAsyncTask(string assetURL)
    {
        var handle = _AssetMgr.GetPackage.LoadRawFileAsync(assetURL);
        
        await handle.Task;
        var data = handle.GetRawFileData();
        handle.Release();
        return data;
    }

    public byte[] GetRawAssetBytes(string assetURL)
    {
        Debug.Log($"GetRawAssetBytes :{assetURL}");
        var handle = _AssetMgr.GetPackage.LoadRawFileSync(assetURL);        
        var data = handle.GetRawFileData();
        handle.Release();
        return data;
    }

    public string GetRawAssetText(string assetURL)
    {
        var handle = _AssetMgr.GetPackage.LoadRawFileSync(assetURL);        
        var data = handle.GetRawFileText();
        handle.Release();
        return data;
    }
    
    public void LoadScene(string sceneURL, bool isAdditive,bool activeOnload,Action onBegin,Action onEnd,Action<float> onLoading)
    {
        LoadSceneAsync(sceneURL, isAdditive, activeOnload, onBegin, onEnd, onLoading).ToObservable().Subscribe();
    }
    #endregion
   

   
    #region 资源释放

    public void ReleaseAsset(string assetUrl)
    {
        _loadedAssets.Remove(assetUrl);
        
        _AssetMgr.ReleaseAsset(assetUrl);
    }

    public void DisposeAll()
    {
        foreach (var assetUrl in _loadedAssets)
        {
            _AssetMgr.ReleaseAsset(assetUrl);
        }
        _loadedAssets.Clear();
    }

    #endregion
   

    #region 资源加载

    private UnityEngine.Object GetAssetSync(string assetPath)
    {
        var handle = _AssetMgr.GetAssetHandle(assetPath,false);
        return handle.AssetObject;
    }
    
    private void GetAssetAsync(string assetPath, Action<UnityEngine.Object > callback)
    {
        var handle = _AssetMgr.GetAssetHandle(assetPath,true);

        if (handle.IsDone)
        {
            Debug.Log("IsDone");
            callback?.Invoke(handle.AssetObject );
        }
        else
        {
            handle.Completed += (self) =>
            {
                Debug.Log($"Complete {self.AssetObject}");
                callback?.Invoke(self.AssetObject );
            };    
        }
    }
    private async Task<UnityEngine.Object> GetAssetTask(string assetPath)
    {        
        var handle = _AssetMgr.GetAssetHandle(assetPath,true);
        await handle.Task;
        var asset = handle.AssetObject;
        return asset;
    }
    
    IEnumerator LoadSceneAsync(string assetPath,bool isAdditive,bool activeOnLoad , Action onBegin ,Action onEnd ,Action<float> onLoading )
    {
        var sceneMode = isAdditive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single;
        var handle = _AssetMgr.GetPackage.LoadSceneAsync(assetPath,sceneMode,activeOnLoad);
        onBegin?.Invoke();
        while (!handle.IsDone)
        {
            onLoading?.Invoke(handle.Progress);
            yield return null;
        }
        onEnd?.Invoke();
    }
    #endregion
}