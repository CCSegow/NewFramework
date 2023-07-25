using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using YooAsset;
using ZFramework.Core;

public class YooAssetManagerComponent : AssetManagerComponent
{
    public enum E_AssetMode
    {
        [LabelText("编辑器模式")]
        Editor,
        [LabelText("离线模式（不需要热更资源）")]
        Offline,
        [LabelText("联网模式（需要热更资源）")]
        Host
    }
    [SerializeField]
    private string DefaultPackage = "DefaultPackage";

    public E_AssetMode AssetMode;
#if UNITY_EDITOR
    public bool IsRebuildEditorAsset;
#endif
    

    [SerializeField,LabelText("默认下载地址"),ShowIf("AssetMode",E_AssetMode.Host)]
    private string DefaultHostServer = "http://127.0.0.1/CDN1/Android/v1.0";
    [SerializeField,LabelText("备用下载地址"),ShowIf("AssetMode",E_AssetMode.Host)]
    private string FallbackHostServer = "http://127.0.0.1/CDN2/Android/v1.0";
    
    [SerializeField,LabelText("最大同时下载数"),ShowIf("AssetMode",E_AssetMode.Host)]
    int downloadingMaxNum = 10;
    [SerializeField,LabelText("失败重试次数"),ShowIf("AssetMode",E_AssetMode.Host)]
    int failedTryAgain = 3;
    [SerializeField,LabelText("超时时间"),ShowIf("AssetMode",E_AssetMode.Host)]
    int timeOut = 60;

    private AssetsPackage _defaultPackage;

    public AssetsPackage GetPackage => _defaultPackage;

    private string _packageVersion;

    private Action _onFinishCallback;
    private Dictionary<string, AssetOperationHandle> _assetOperationHandles;

   
    protected override void OnInit()
    {
        Debug.Log("Init");
        _assetOperationHandles = new Dictionary<string, AssetOperationHandle>();
    }

    public override void BeginAssetUpdate(Action onFinish)
    {
        _onFinishCallback = onFinish;
        StartCoroutine(DoAssetsUpdate());
    }

 

    private void OnDisable()
    {
        YooAssets.Destroy();
    }

    IEnumerator TestWebSever()
    {
        var fileName = "PatchManifest_DefaultPackage.version";
        var www = UnityWebRequest.Get($"{DefaultHostServer}/{fileName}");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"result = {www.result}, error: {www.error}");
            yield break;
        }

        var text = www.downloadHandler.text;
        Debug.Log(text);
    }

    IEnumerator DoAssetsUpdate()
    {
        //初始化YooAsset系统资源
        YooAssets.Initialize();
        yield return null;
        _defaultPackage = YooAssets.CreateAssetsPackage(DefaultPackage);
        YooAssets.SetDefaultAssetsPackage(_defaultPackage);

#if UNITY_EDITOR
        AssetMode = E_AssetMode.Editor;
#elif OFFLINE_MODE
        //单机模式
        AssetMode = E_AssetMode.Offline;
#endif
        
        Debug.Log($"AssetMode = {AssetMode}");
        //初始化资源包
        //_assetsPackage.InitializeAsync();
        if (AssetMode == E_AssetMode.Editor)
        {            
            yield return Init_EditorMode();
        }        
        else if(AssetMode == E_AssetMode.Offline)
        {
            yield return Init_OfflineMode();
        }
        else if(AssetMode == E_AssetMode.Host)
        {
            Debug.Log("1.初始化参数");
            //1.初始化参数
            yield return Init_HostMode();
            var operation = _defaultPackage.UpdatePackageVersionAsync(30);
            yield return operation;
            //获取远端资源版本成功，说明当前网络畅通，可以走正常更新流程
            if (operation.Status == EOperationStatus.Succeed)
            {
               
                Debug.Log("2.更新补丁清单");
                //2.更新补丁清单
                //对于联机模式，在更新补丁之前，需要先获取一个资源版本
                //资源版本可以通过YooAssets提供的接口来更新，也可以通过Http访问游戏服务器来获取
                var operation_UpdateStaticVersion = _defaultPackage.UpdatePackageVersionAsync();
                yield return operation_UpdateStaticVersion;
                if (operation_UpdateStaticVersion.Status == EOperationStatus.Succeed)
                {
                    //更新成功
                    _packageVersion = operation_UpdateStaticVersion.PackageVersion;
                }
                else
                {
                    //更新失败
                    Debug.LogError(operation_UpdateStaticVersion.Error);
                    yield break;
                }
                
                //3.更新资源清单 （对于联机模式，在获取到资源版本号后，就可以更新资源清单了）
                var operation_UpdatePatchManifest = _defaultPackage.UpdatePackageManifestAsync(_packageVersion);
                yield return operation_UpdatePatchManifest;
                if (operation_UpdatePatchManifest.Status != EOperationStatus.Succeed)
                {
                    //更新失败
                    Debug.LogError($"更新失败　{operation_UpdatePatchManifest.Error}");
                    yield break;
                }
                //4.下载资源 
                yield return Download();    
            }
            else
            {
                //获取远端资源版本失败，说明当前无网络链接
                //在游戏正常开始前，需要验证本地清单内容的完整性
                yield return NetworkError();
            }
        }
        else
        {
            Debug.LogWarning($"未定义模式 {AssetMode}");
        }
        //开始游戏
        _onFinishCallback?.Invoke();
    }

    

    IEnumerator NetworkError()
    {
        var version = _defaultPackage.GetPackageVersion();
        var operation = _defaultPackage.DownloadPackageAsync(version);
        yield return operation;
        if (operation.Status != EOperationStatus.Succeed)
        {
            //提示玩家，请检查本地网络，有新的游戏内容需要更新
            yield break;
        }
        
        var downloader = _defaultPackage.CreatePatchDownloader(downloadingMaxNum, failedTryAgain, timeOut);
        if (downloader.TotalDownloadCount > 0)
        {
            //提示玩家，请检查本地网络，有新的游戏内容需要更新
            yield break;
        }
    }

    #region 初始化
    IEnumerator Init_EditorMode()
    {
        var initParameters = new EditorSimulateModeParameters();
        
        initParameters.SimulatePatchManifestPath = EditorSimulateModeHelper.SimulateBuild(DefaultPackage);
        
        //TODO 不用每次都Build
        // if (IsRebuildEditorAsset)
        // {
        //     initParameters.SimulatePatchManifestPath = "";
        // }
        

        yield return _defaultPackage.InitializeAsync(initParameters);
    }

    //此模式需要构建资源包
    IEnumerator Init_OfflineMode()
    {
        var initParameters = new OfflinePlayModeParameters();
        yield return _defaultPackage.InitializeAsync(initParameters);
    }

    IEnumerator Init_HostMode()
    {
        var initParameters = new HostPlayModeParameters();
        initParameters.QueryServices = new QueryStreamingAssetsFileServices();
        initParameters.DefaultHostServer = DefaultHostServer;
        initParameters.FallbackHostServer = FallbackHostServer;
        yield return _defaultPackage.InitializeAsync(initParameters);
    }
    #endregion
    
    #region 补丁包下载

    private IEnumerator Download()
    {
        var downloader = _defaultPackage.CreatePatchDownloader(downloadingMaxNum, failedTryAgain, timeOut);
        
        //没有需要下载的资源
        if (downloader.TotalDownloadCount == 0)
        {
            yield break;
        }

        //需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadCount;
        
        //注册回调方法
        downloader.OnDownloadErrorCallback = OnDownloadError;
        downloader.OnDownloadProgressCallback = OnDownloadProgress;
        downloader.OnDownloadOverCallback = OnDownloadOver;
        downloader.OnStartDownloadFileCallback = OnStartDownloadFile;
        
        //开启下载
        downloader.BeginDownload();
        yield return downloader;

        //检测下载结果
        if (downloader.Status == EOperationStatus.Succeed)
        {
            //下载成功
        }
        else
        {
            //下载失败
        }
    }

    //开始下载
    private void OnStartDownloadFile(string filename, long sizebytes)
    {
        Debug.Log($"开始下载 {filename},文件大小 :{sizebytes}");
    }

    //下载完成
    private void OnDownloadOver(bool issucceed)
    {
        var msg = issucceed ? "成功" : "失败";
        Debug.Log($"下载 {msg}");
    }

    //下载中
    private void OnDownloadProgress(int totaldownloadcount, int currentdownloadcount, long totaldownloadbytes, long currentdownloadbytes)
    {
        Debug.Log($"文件总数 {totaldownloadcount},已下载文件 :{currentdownloadcount},{currentdownloadbytes}/{totaldownloadbytes}");
    }

    //下载出错
    void OnDownloadError(string filename, string error)
    {
        Debug.Log($"下载 {filename} 失败，{error}");
    }

    #endregion

    #region 对外接口
    public override I_AssetServant GetServant()
    {
        //YooAssetServant servant = new YooAssetServant(GetAssetSync,GetAssetAsync,GetSceneTask,GetAssetTask,GetRawAssetTask,ReleaseAsset);
        YooAssetServant servant = new YooAssetServant(this);
        return servant;
    }
    
    public AssetOperationHandle GetAssetHandle(string assetURL,bool isAsync)
    {
        if (!_assetOperationHandles.TryGetValue(assetURL,out var handle))
        {
            if (isAsync)
            {
                handle = _defaultPackage.LoadAssetAsync<UnityEngine.Object >(assetURL);
            }
            else
            {
                handle = _defaultPackage.LoadAssetSync<UnityEngine.Object>(assetURL);
            }
            _assetOperationHandles.Add(assetURL,handle);
        }

        return handle;
    }


    private UnityEngine.Object GetAssetSync(string assetPath)
    {
        var handle = GetAssetHandle(assetPath,false);
        return handle.AssetObject;
    }
    private void GetAssetAsync(string assetPath, Action<UnityEngine.Object > callback)
    {
        var handle = GetAssetHandle(assetPath,true);

        if (handle.IsDone)
        {
            Debug.Log("IsDone");
            callback?.Invoke(handle.AssetObject );
        }
        else
        {
            handle.Completed += (AssetOperationHandle self) =>
            {
                Debug.Log($"Complete {self.AssetObject}");
                callback?.Invoke(self.AssetObject );
            };    
        }
    }

    private async Task<byte[]> GetRawAssetTask(string assetPath)
    {
        Debug.Log($"GetAssetTask {assetPath}");
        var handle = _defaultPackage.LoadRawFileAsync(assetPath);
        
        await handle.Task;
        var data = handle.GetRawFileData();
        handle.Release();
        return data;
    }

    private async Task<UnityEngine.Object> GetAssetTask(string assetPath)
    {        
        var handle = GetAssetHandle(assetPath,true);
        await handle.Task;
        var asset = handle.AssetObject;
        return asset;
    }
    
    private void GetSceneTask(string assetPath, bool isAdditive, bool activeOnLoad, Action onBegin = null,
        Action onEnd = null, Action<float> onLoading = null)
    {
        StartCoroutine(LoadSceneAsync(assetPath, isAdditive, activeOnLoad, onBegin, onEnd, onLoading));
    }

    IEnumerator LoadSceneAsync(string assetPath,bool isAdditive,bool activeOnLoad , Action onBegin ,Action onEnd ,Action<float> onLoading )
    {
        var sceneMode = isAdditive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single;
        var handle = _defaultPackage.LoadSceneAsync(assetPath,sceneMode,activeOnLoad);
        onBegin?.Invoke();
        while (!handle.IsDone)
        {
            onLoading?.Invoke(handle.Progress);
            yield return null;
        }
        onEnd?.Invoke();
    }


    //卸载资源
    public void ReleaseAsset(string assetPath)
    {
        if (_assetOperationHandles.TryGetValue(assetPath, out var handle))
        {
            //Debug.LogWarning($"Release {assetPath}");
            handle.Release();   
        }
        
        _defaultPackage.UnloadUnusedAssets();
    }

    //释放资源包
    private void ReleasePackage()
    {
        _defaultPackage.UnloadUnusedAssets();
    }

    #endregion
    
    private class QueryStreamingAssetsFileServices:IQueryServices
    {
        public bool QueryStreamingAssets(string fileName)
        {
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");
        }
    }
}

