using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;

namespace BuildTool
{
    public enum E_CpoyBuildAssetOption
    {
        [LabelText("不拷贝")]
        None = 0,

        [LabelText("清空后拷贝全部")]
        ClearAndCopyAll,

        [LabelText("清空后按照资源标签拷贝")]
        ClearAndCopyByTags,

        [LabelText("不清空，直接拷贝全部")]
        OnlyCopyAll,

        [LabelText("不清空，按照资源标签拷贝")]
        OnlyCopyByTags,
    }

    [System.Serializable]
    public class BundleBuildSetting
    {
        [LabelText("构建模式")]
        public EBuildMode BuildMode;
        [LabelText("构建版本")]
        public string Version;
        [LabelText("构建的资源包名称")]
        public string BuildPackage = "DefaultPackage";
        [LabelText("覆盖本地已构建的同版本资源包")]
        public bool IsCoverExit;
        [LabelText("构建完成后拷贝到本地服务器")]
        public bool IsCopyToLocalServer = true;

   

        [LabelText("构建后拷贝随包资源")]
        public E_CpoyBuildAssetOption CopyBuildAssetOption;
        [LabelText("需要拷贝的标签"),ShowIf("@CopyBuildAssetOption == E_CpoyBuildAssetOption.OnlyCopyByTags || CopyBuildAssetOption == E_CpoyBuildAssetOption.ClearAndCopyByTags" )]
        public string CopyTags;

        public string ServerPath;

        private string GetGitProjectPath;
        
        [Button]
        public void Build()
        {
            if (IsCoverExit)
            {
                var outputPath = GetOutputPath;
                Debug.Log($"outputPath = {outputPath}");
                if (Directory.Exists(outputPath))
                {
                    Directory.Delete(outputPath,true);
                    Debug.Log("删除上一次打包的缓存");
                }
            }

            var _buildTarget = EditorUserBuildSettings.activeBuildTarget;
            
            string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultOutputRoot();
            BuildParameters buildParameters = new BuildParameters();
            buildParameters.OutputRoot = defaultOutputRoot;
            buildParameters.BuildTarget = _buildTarget;
            buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline; //AssetBundleBuilderSettingData.Setting.BuildPipeline;
            buildParameters.BuildMode = BuildMode;
            buildParameters.PackageName = BuildPackage;
            buildParameters.PackageVersion = Version;
            buildParameters.VerifyBuildingResult = true;
            buildParameters.EncryptionServices = Activator.CreateInstance<EncryptionNone>();
            buildParameters.CompressOption = ECompressOption.LZ4; // AssetBundleBuilderSettingData.Setting.CompressOption;
            buildParameters.OutputNameStyle = EOutputNameStyle.HashName; //AssetBundleBuilderSettingData.Setting.OutputNameStyle;
            buildParameters.CopyBuildinFileOption = (ECopyBuildinFileOption)CopyBuildAssetOption; //AssetBundleBuilderSettingData.Setting.CopyBuildinFileOption;
            buildParameters.CopyBuildinFileTags = "";//AssetBundleBuilderSettingData.Setting.CopyBuildinFileTags;

            if (AssetBundleBuilderSettingData.Setting.BuildPipeline == EBuildPipeline.ScriptableBuildPipeline)
            {
                buildParameters.SBPParameters = new BuildParameters.SBPBuildParameters();
                buildParameters.SBPParameters.WriteLinkXML = true;
            }

            var builder = new AssetBundleBuilder();
            var buildResult = builder.Run(buildParameters);
            if (buildResult.Success)
            {
                EditorUtility.RevealInFinder(buildResult.OutputPackageDirectory);
            }

            if (IsCopyToLocalServer)
            {
                CopyToLoadServer();
            }
        }

                
        //[Button("拷贝到本地服务器")]
        void CopyToLoadServer()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetPath = GetGitProjectPath;
            targetPath = Path.Combine(targetPath, $"{ServerPath}/{buildTarget}/{BuildPackage}/{Version}");

            var srcPath = GetOutputPath;
            Debug.Log("targetPath = "+targetPath);
            Debug.Log("srcPath = "+srcPath);
            
            CopyFolder(srcPath,targetPath);
            Debug.Log("拷贝完成");
        }     

        string GetOutputPath
        {
            get
            {
                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
                string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultOutputRoot();
                var srcPath = Path.Combine(defaultOutputRoot, $"{BuildPackage}/{buildTarget}/{Version}");
                return srcPath;
            }
        }

        [Button("打开构建输出目录")]
        void OpenBuildFolder()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultOutputRoot();
            var srcPath = Path.Combine(defaultOutputRoot, $"{BuildPackage}/{buildTarget}/{Version}");
            Debug.Log($"{srcPath}");
            if (Directory.Exists(srcPath))
            {
                EditorUtility.RevealInFinder(srcPath);
            }
            else
            {
                EditorUtility.RevealInFinder(defaultOutputRoot);
            }
        }
        
        [Button("打开服务器拷贝目录")]
        void OpenServerFolder()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var serverPath = Path.Combine(GetGitProjectPath,ServerPath);
            var targetPath = Path.Combine(serverPath, $"{buildTarget}/{BuildPackage}/{Version}");
            EditorUtility.RevealInFinder(targetPath);
            if (Directory.Exists(targetPath))
            {
                EditorUtility.RevealInFinder(targetPath);
            }
            else
            {
                EditorUtility.RevealInFinder(serverPath);
            }
        }

        void CopyFolder(string srcPath,string targetPath)
        {
            
        }

    }
    
    public class EncryptionNone : IEncryptionServices
    {
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.Normal;
            return result;
        }
    }
}