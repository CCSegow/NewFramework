using System.IO;
using UnityEditor;
using UnityEngine;

namespace Luban.Editor
{
    public static class LubanTool
    {
        [MenuItem("Tools/导表工具/导出二进制")]
        public static void ExportConfigBinary()
        {
            string fileName = "gen_code_bin.bat";
       
            string local = PathUtil.ProjectPath + "../Luban/";
            string batPath = Path.Combine(local,fileName);
            Debug.Log(File.Exists(batPath));
            BatUtil.RunBat(batPath,"",local);
        }

        [MenuItem("Tools/导表工具/导出Json")]
        public static void ExportConfigJson()
        {
            string fileName = "gen_code_json.bat";

            string local = PathUtil.ProjectPath + "../Luban/";
            string batPath = Path.Combine(local,fileName);
            Debug.Log(File.Exists(batPath));
            BatUtil.RunBat(batPath,"",local);
        }
        
        [MenuItem("Tools/导表工具/拷贝生成代码到工程（不同方式导出的配置，会自动识别读取方式）")]
        public static void CopyCodeToProject()
        {
            string srcFileName = PathUtil.ProjectPath + "../Luban/Configs/Gen";
            string dstFileName = PathUtil.ProjectPath + "Assets/GamePlay/Scripts/Hotfix/Configs/Gen";
            IOUtils.CopyFolder(srcFileName, dstFileName,"*.cs",true);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("导入配置代码结束");
        }

        [MenuItem("Tools/导表工具/拷贝二进制到工程")]
        public static void CopyBinaryToProject()
        {
            string srcFileName = PathUtil.ProjectPath + "../Luban/Configs/output_bytes";
            string dstFileName = PathUtil.ProjectPath + "Assets/Bundles/Configs/Bytes";            
            IOUtils.CopyFolder(srcFileName, dstFileName,"*.bytes",true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("导入二进制配置结束");
        }
        
        [MenuItem("Tools/导表工具/拷贝Json到工程")]
        public static void CopyCJsonToProject()
        {
            string srcFileName = PathUtil.ProjectPath + "../Luban/Configs/output_json";
            string dstFileName = PathUtil.ProjectPath + "Assets/Bundles/Configs/Jsons";
            IOUtils.CopyFolder(srcFileName, dstFileName,"*.json",true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("导入json配置结束");
        }
    }
}