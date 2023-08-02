using System;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

namespace BuildTool
{
    public class AssetBuilder
    {
        /// <summary>
        /// 命令行调用
        /// </summary>
        public static void BuildByCommand()
        {
            var buildParams = GetBuildPackageName();
            if (buildParams == null)
            {
                Debug.LogError("构建失败");
                return;
            }

            Debug.Log("OutputRoot:"+buildParams.OutputRoot);
            Debug.Log("BuildTarget:"+buildParams.BuildTarget);
            Debug.Log("BuildPipeline:"+buildParams.BuildPipeline);
            Debug.Log("BuildMode:"+buildParams.BuildMode);
            Debug.Log("PackageName:"+buildParams.PackageName);
            Debug.Log("PackageVersion:"+buildParams.PackageVersion);
            Debug.Log("VerifyBuildingResult:"+buildParams.VerifyBuildingResult);
            Debug.Log("CompressOption:"+buildParams.CompressOption);
            Debug.Log("OutputNameStyle:"+buildParams.OutputNameStyle);
            Debug.Log("CopyBuildinFileOption:"+buildParams.CopyBuildinFileOption);
            Run(buildParams);
        }

        static void Run(BuildParameters buildParams)
        {
            Debug.Log($"开始构建 : ");
            // 执行构建
            AssetBundleBuilder builder = new AssetBundleBuilder();
            var buildResult = builder.Run(buildParams);
            if (buildResult.Success)
            {
                Debug.Log($"构建成功 : {buildResult.OutputPackageDirectory}");
            }
            else
            {
                Debug.LogError($"构建失败 : {buildResult.FailedInfo}");
            }
        }

        // 从构建命令里获取参数示例
        private static BuildParameters GetBuildPackageName()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            BuildParameters buildParameters = new BuildParameters();
            foreach (string s in args)
            {
                if (s.Contains("--outputRoot:"))
                {
                    string outputRoot = s.Split(':')[1];
                    buildParameters.OutputRoot = outputRoot;
                }               

                if (s.Contains("--buildTarget:"))
                {
                    string target = s.Split(':')[1];
                    if (Enum.TryParse(target,out BuildTarget buildTarget))
                    {
                        buildParameters.BuildTarget = buildTarget;
                    }
                    else
                    {
                        Debug.LogError($"未定义目标类型 {target}");
                        return null;
                    }
                }
                
               
                if (s.Contains("--buildPipeline:"))
                {
                    string buildPipeline = s.Split(':')[1];   
                    if (Enum.TryParse(buildPipeline,out EBuildPipeline pipeline))
                    {
                        buildParameters.BuildPipeline = pipeline;
                    }
                    else
                    {
                        Debug.LogError($"未定义构建管线类型 {buildPipeline}");
                        return null;
                    }
                }
                
                if (s.Contains("--buildMode:"))
                {
                    string mode = s.Split(':')[1];
                    if (Enum.TryParse(mode,out EBuildMode buildMode))
                    {
                        buildParameters.BuildMode = buildMode;
                    }
                    else
                    {
                        Debug.LogError($"未定义构建类型 {mode}");
                        return null;
                    }
                }
                

                if (s.Contains("--defaultPackage:"))
                {
                    string defaultPackage = s.Split(':')[1];
                    buildParameters.PackageName = defaultPackage;
                }
                
                if (s.Contains("--packageVersion:"))
                {
                    string packageVersion = s.Split(':')[1];
                    buildParameters.PackageVersion = packageVersion;
                }
                
                if (s.Contains("--verifyBuildingResult:"))
                {
                    string verifyBuildingResult = s.Split(':')[1];
                    if (Boolean.TryParse(verifyBuildingResult,out var result))
                    {
                        buildParameters.VerifyBuildingResult = result;
                    }
                    else
                    {
                        Debug.LogError($"verifyBuildingResult ，无效参数 {verifyBuildingResult}，必须是 boolean");
                        return null;
                    }
                }
                
                buildParameters.CopyBuildinFileOption = ECopyBuildinFileOption.None;
                if (s.Contains("--copyBuildinFileOption:"))
                {
                    string copyBuildinFileOption = s.Split(':')[1];
                    if (Enum.TryParse(copyBuildinFileOption,out ECopyBuildinFileOption option))
                    {
                        buildParameters.CopyBuildinFileOption = option;
                    }
                    else
                    {
                        Debug.LogError($"copyBuildinFileOption ，无效参数 {copyBuildinFileOption}");
                        return null;
                    }
                }
            }
            
            buildParameters.CompressOption = ECompressOption.LZ4;
            buildParameters.OutputNameStyle = EOutputNameStyle.HashName;
            if (string.IsNullOrEmpty(buildParameters.OutputRoot))
            {
                buildParameters.OutputRoot = AssetBundleBuilderHelper.GetDefaultOutputRoot();
            }
            
            return buildParameters;
        }
    }
}