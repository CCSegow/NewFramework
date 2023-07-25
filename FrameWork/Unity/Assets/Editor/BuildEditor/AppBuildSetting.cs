using System.Collections;
using System.IO;
using BuildTool;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace BuildTool
{
    public enum EBuildTarget
    {
        [LabelText("安卓")]
        Android,
        [LabelText("iPhone")]
        IPhone,
        [LabelText("Window")]
        Window
    }
    
    public enum E_PlayMode
    {
        [LabelText("单机")]
        OfflinePlayMode,
        [LabelText("联网")]
        HostPlayMode,
    }

    [System.Serializable]
    public class AppBuildSetting
    {
        [LabelText("公司名")]
        public string companyName = "YimiStudio";
        [LabelText("应用名")]
        public string appName = "测试";
        [LabelText("包名"),PropertyTooltip("iOS、Android公用，字符串，一般格式为com.company.game，iOS里用于开发者证书")]
        public string applicationIdentifier = "com.YimiStudio.demo";
        
        //X.Y.Z.W
        //X 默认为1不用管，除非太大变化；
        //Y:商店版本（提交到应用商店，强更包，大更新，游戏一般一个月有一次大更新，每个月会提交一次商店，每次提交Y+1）
        //Z:也是商店版本，用来修复商店版本的Bug，一个版本周期内，SDK有问题，或C#层有Bug，需要强更，但是不想影响Y值
        //W:热更版本，每次热更加1
        //版本号 iOS、Android公用，字符串，一般格式为1.2.3，用于显示给用户的版本信息
        [LabelText("应用版本(显示给用户看)"),ValidateInput("IsAppVersionValidate"),OnValueChanged("UpdateBundleVersionCode"),
         PropertyTooltip("X 默认为1，Y:商店版本，Z:商店版本修复版本，W:热更版本")]
        public string appVersion = "1.0.0.0";
        [LabelText("内部版本号(整包更新判断)"),ReadOnly]
        public string bundleVersionCode = "10000";// Z + Y * 1000 + X * 10000
        
        [LabelText("Dev模式")]
        public bool DevelopBuild;

        [LabelText("运行模式")]
        public E_PlayMode PlayMode;
        //TODO添加勾选是否增加商店版本号，热更版本号
        

        public EBuildTarget BuildTarget;
        
        [ValueDropdown("GetOutputPath")]
        public string OutputPath = "../Builds/Test";

        private IEnumerable GetOutputPath = new ValueDropdownList<string>()
        {
            {"Test","../Builds/Test"},
            {"Dev","../Builds/Dev"},
            {"Release","../Builds/Release"},
        };

        bool IsAppVersionValidate(string data,ref string errorMsg)
        {
            string[] numbers = data.Split('.');
            if (numbers.Length != 4)
            {
                errorMsg = "必须是 X.Y.Z.W ";
                return false;
            }

            foreach (var num in numbers)
            {
                if (!int.TryParse(num,out var v))
                {
                    errorMsg = "必须是数字 ";
                    return false;
                }
            }

            return true;
        }

        void UpdateBundleVersionCode()
        {
            string msg = "";
            if (!IsAppVersionValidate(appVersion,ref msg ))
            {
                return;
            }

            string[] numbers = appVersion.Split('.');
            int x = int.Parse(numbers[0]);
            int y = int.Parse(numbers[1]);
            int z = int.Parse(numbers[2]);
            int versionCode = z + y * 1000 + x * 10000;
            bundleVersionCode = versionCode.ToString();
        }

        [Button]
        public void Build()
        {
            var path = Path.Combine($"{PathUtil.ProjectPath}",OutputPath);
            DirectoryInfo dir = new DirectoryInfo(path);
            
            AppBuildParams buildParams = new AppBuildParams();
            buildParams.companyName = companyName;
            buildParams.appName = appName;
            buildParams.version = appVersion;
            buildParams.bundleVersionCode = bundleVersionCode;
            buildParams.applicationIdentifier = applicationIdentifier;
            buildParams.outPutPath = dir.FullName;
            buildParams.IsDevelopBuild = DevelopBuild;
            if (BuildTarget == EBuildTarget.Android)
            {
                buildParams.BuildTarget = UnityEditor.BuildTarget.Android;
            }
            else if (BuildTarget == EBuildTarget.IPhone)
            {
                buildParams.BuildTarget = UnityEditor.BuildTarget.iOS;
            }
            else if (BuildTarget == EBuildTarget.Window)
            {
                buildParams.BuildTarget = UnityEditor.BuildTarget.StandaloneWindows64;
            }
            else
            {
                Debug.LogError($"未定义平台 {BuildTarget}");
                return;
            }

            buildParams.ScriptSymbols = string.Join(";",SymbolsDefs.symbols);

            BuildTool.AppBuilder.RunBuilder(buildParams);
            EditorUtility.RevealInFinder(dir.FullName);
        }


        
        [System.Serializable]
        public struct SymbolsDefine
        {
            public BuildTarget buildTarget;
            public string[] symbols;


            [Button("应用")]
            public void Apply()
            {
                if (!EditorUtility.DisplayDialog("注意","将覆盖当前设置","确认","取消"))
                {
                    return;
                }
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPipeline.GetBuildTargetGroup(buildTarget), symbols);
            }
            [Button("初始化")]
            public void Get()
            {
                if (!EditorUtility.DisplayDialog("注意","将覆盖当前设置","确认","取消"))
                {
                    return;
                }

                var str = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(buildTarget)));
                symbols = str.Split(';');
            }

        
        }

        [HideLabel,FoldoutGroup("脚本宏定义")]
        public SymbolsDefine SymbolsDefs;

   

    }
}