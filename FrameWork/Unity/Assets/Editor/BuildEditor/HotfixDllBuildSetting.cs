using System.Collections;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace BuildTool
{
    [System.Serializable]
    public class HotfixDllBuildSetting
    {
        [LabelText("生成dll"),ValueDropdown("GetCommands")] 
        public string BuildCommand = "HybridCLR/Generate/All";

        public BuildTarget BuildTarget;
        [LabelText("复制AOT元数据dll")]
        public bool CopyAOTDll;
        private IEnumerable GetCommands = new ValueDropdownList<string>
        {
            {"不生成",""},
            {"生成所有","HybridCLR/Generate/All"},
            {"只生成热更dll","HybridCLR/CompileDll/ActiveBuildTarget"},
        };

        
        [Button]
        public void Build()
        {
            //生成
            if(BuildCommand != "")
                EditorApplication.ExecuteMenuItem(BuildCommand);
            //拷贝dll到热更资源文件夹
            //BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            
            
            string folder = Path.Combine($"{PathUtil.ProjectPath}HybridCLRData/HotUpdateDlls/",BuildTarget.ToString());
            // Debug.Log($"{folder}");
            // Debug.Log(Directory.Exists(folder));

            string resourceFolder = Path.Combine(Application.dataPath,"Bundles/Code/");
            
            // Debug.Log($"{resourceFolder}");
            // Debug.Log(Directory.Exists(resourceFolder));
            // 热更dll
            IOUtils.CopyFile(Path.Combine(folder,"Hotfix.dll"),Path.Combine(resourceFolder,"Hotfix.dll.bytes"));
            
            //AOT 元数据注入
            if (CopyAOTDll)
            {
                string aotFolder = Path.Combine($"{PathUtil.ProjectPath}HybridCLRData/AssembliesPostIl2CppStrip/",BuildTarget.ToString());
                // Debug.Log($"{aotFolder}");
                // Debug.Log(Directory.Exists(aotFolder));
                IOUtils.CopyFile(Path.Combine(aotFolder,"mscorlib.dll"),Path.Combine(resourceFolder,"mscorlib.dll.bytes"));
                IOUtils.CopyFile(Path.Combine(aotFolder,"System.Core.dll"),Path.Combine(resourceFolder,"System.Core.dll.bytes"));
                IOUtils.CopyFile(Path.Combine(aotFolder,"System.dll"),Path.Combine(resourceFolder,"System.dll.bytes"));
                IOUtils.CopyFile(Path.Combine(aotFolder,"ZFrameWork.Core.dll"),Path.Combine(resourceFolder,"ZFrameWork.Core.dll.bytes"));
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

      
    }
}