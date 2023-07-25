using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HybridCLR;
using Sirenix.OdinInspector;
using UnityEngine;
using ZFramework.Core;

public class HybridCLRHotfixComponent : HotFixComponent
{
    [LabelText("热更dll路径"),FilePath]
    public string DllAssetPath;
    
    private static List<string> AOTDllList = new List<string>()
    {
        "System.dll",
        "mscorlib.dll",
        "System.Core.dll", // 如果使用了Linq，需要这个
        "ZFrameWork.Core.dll",
        // StompyRobot.SRF.dll.bytes // 开发者包才需要加上这个调试插件        
    };
    
    private static Assembly _hotfixAssembly;

    private Action _onFinish;
    protected override void OnInit()
    {
        
    }

    public override void BeginLoadHotfix(Action onFinish)
    {
        _onFinish = onFinish;
        InitHybridCLR();
    }

    async void InitHybridCLR()
    {
        var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
        var servant = assetComponent.GetServant();
        await LoadMetadataForAOTAssembly(servant);
#if !UNITY_EDITOR
            var data = await servant.GetRawAssetAsyncTask(DllAssetPath);
            _hotfixAssembly = Assembly.Load(data);
#else
        _hotfixAssembly = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
#endif
        servant.DisposeAll();
        //返回主逻辑
        _onFinish.Invoke();
    }

    async Task LoadMetadataForAOTAssembly(I_AssetServant servant)
    {
        Debug.Log("补充元数据");
        foreach (var aotDllName in AOTDllList)
        {
            var assetPath = $"Assets/Bundles/Code/{aotDllName}.bytes";
            byte[] dllBytes = await servant.GetRawAssetAsyncTask(assetPath);
            var err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.Log($"LoadMetadataForAOTAssembly:{assetPath}. ret:{err}");
        }
            
    }
}