using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace BuildTool
{
    public class AppBuildParams
    {
        public string companyName = "WJ";
        public string appName = "测试";
        public string version = "1.0.0.0";
        public string bundleVersionCode;
        public string applicationIdentifier = "com.WJ.zombie";
        public string outPutPath;
        public bool IsDevelopBuild;
        public BuildTarget BuildTarget;
        public string ScriptSymbols;
    }
    public class AppBuilder
    {
        public static void RunBuilder(AppBuildParams appBuildParams)
        {
            if(appBuildParams == null)
                return;
            BuildPlayerOptions opt = new BuildPlayerOptions();

            opt.scenes = new string[]
            {
                "Assets/Bundles/Scenes/Launch.unity",
                
            };
            
            opt.options = BuildOptions.None;
            bool isDev = appBuildParams.IsDevelopBuild;
            if (isDev)
            {
                opt.options |= BuildOptions.Development;
                opt.options |= BuildOptions.AllowDebugging;
                opt.options |= BuildOptions.ConnectWithProfiler;
            }

            if (!Directory.Exists(appBuildParams.outPutPath))
            {
                Directory.CreateDirectory(appBuildParams.outPutPath);
            }
            DirectoryInfo dir = new DirectoryInfo(appBuildParams.outPutPath);
            
            SetPackageData(appBuildParams);
            
            //包名
            var version_string = PlayerSettings.bundleVersion;
            version_string = version_string.Replace('.', '_');
            string apkName = $"{dir.Name.ToLower()}_{version_string}";
            if (appBuildParams.BuildTarget == BuildTarget.Android)
            {
                opt.locationPathName = appBuildParams.outPutPath + $"/{apkName}.apk";    
            }else if (appBuildParams.BuildTarget == BuildTarget.iOS)
            {
                opt.locationPathName = appBuildParams.outPutPath + $"/{apkName}.ipa";
            }
            else if (appBuildParams.BuildTarget == BuildTarget.StandaloneWindows64)
            {
                opt.locationPathName = appBuildParams.outPutPath + $"/{apkName}.exe";    
            }
            else
            {
                Debug.LogError($"未定义BuildTarget {appBuildParams.BuildTarget}");
                return;
            }
            Debug.Log($" 输出路径 = {opt.locationPathName}");

            
            opt.target = appBuildParams.BuildTarget;


            var oldBuildTarget = BuildUtil.GetActiveBuildTargetGroup();
            var oldSymbols = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(oldBuildTarget));
            SetScriptDefineSymbols(BuildPipeline.GetBuildTargetGroup(appBuildParams.BuildTarget),appBuildParams.ScriptSymbols);
            
            BuildPipeline.BuildPlayer(opt);
            
            //还原
            SetScriptDefineSymbols(oldBuildTarget,oldSymbols);
            
            Debug.Log("Build App Done!!!");
        }
        
        //设置包体参数
        static void SetPackageData(AppBuildParams appBuildParams)
        {            
            PlayerSettings.companyName = appBuildParams.companyName; //公司名            
            PlayerSettings.productName = appBuildParams.appName;//应用名
            
            
            PlayerSettings.applicationIdentifier = appBuildParams.applicationIdentifier;//包名 iOS、Android公用，字符串，一般格式为com.company.game，iOS里用于开发者证书
            
            //特别注意：如果iOS要提审AppStore，那么Bundle Version必须是3位，不可以4位，比如1.2.3.0这样是不行的
            PlayerSettings.bundleVersion = appBuildParams.version;//版本号 iOS、Android公用，字符串，一般格式为1.2.3，用于显示给用户的版本信息
            
            PlayerSettings.Android.bundleVersionCode = int.Parse(appBuildParams.bundleVersionCode); //Android 特有，数字，android版本的内部版本号，可用于区别当前版本是否最新apk，进而整包更新
            PlayerSettings.iOS.buildNumber = appBuildParams.bundleVersionCode; //iOS特有，意义同上
        }
        
        //设置脚本宏定义
        static void SetScriptDefineSymbols(BuildTargetGroup buildTarget,string symbols)
        {
            if (!string.IsNullOrEmpty(symbols))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, symbols);  
            }
        }

        /// <summary>
        /// 设置应用图标（不同渠道可能图标不一样）
        /// </summary>
        void SetAppIcon()
        {
            
        }

        /// <summary>
        /// 设置密钥
        /// </summary>
        void SetKeyStore()
        {
            PlayerSettings.Android.useCustomKeystore = false;//不使用正式签名
            
            PlayerSettings.Android.keystoreName = ""; // 路径
            PlayerSettings.Android.keystorePass = ""; // 密钥密码
            PlayerSettings.Android.keyaliasName = ""; // 密钥别名
            PlayerSettings.Android.keyaliasPass = "";
        }

    
        
        static AppBuildParams GetAppData_ByCommandLine()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            AppBuildParams appBuildParams = new AppBuildParams();
            foreach (var s in args)
            {
                if (s.Contains("--productName:"))
                {
                    //设置app(应用)名字
                    string productName = s.Split(':')[1];
                    appBuildParams.appName = productName;
                }
                if (s.Contains("--version:"))
                {
                    //设置版本号
                    string version = s.Split(':')[1];
                    appBuildParams.version = version;
                }
                if (s.Contains("--buildTarget:"))
                {
                    //构建平台
                    string value = s.Split(':')[1];
                    if (Enum.TryParse(value, out BuildTarget target))
                    {
                        appBuildParams.BuildTarget = target;    
                    }
                    else
                    {
                        Debug.LogError($"未定义构建平台 {value}");
                        return null;
                    }


                }
            }

            return appBuildParams;
        }
        static AppBuildParams GetAppData_ByDefaultTest()
        {
            AppBuildParams appBuildParams = new AppBuildParams();
            return appBuildParams;
        }


        
      
    }
    
}