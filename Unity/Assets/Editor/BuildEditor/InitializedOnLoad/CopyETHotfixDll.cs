using System.IO;
using UnityEditor;
using UnityEngine;

namespace BuildTool
{
    //[InitializeOnLoad]
    public class Startup
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/Res/Code/";
        private const string HotfixDll = "Unity.Hotfix.dll";
        private const string HotfixPdb = "Unity.Hotfix.pdb";
        static Startup()
        {
            var srcFileName = Path.Combine(ScriptAssembliesDir, HotfixDll);
            var dstFileName = Path.Combine(CodeDir, "Hotfix.dll.bytes");
            if (FileUtil.CompareByByteArray(srcFileName, dstFileName))
            {
                Debug.Log($"Hotfix.dll内容相同，不需要拷贝");
                return;
            }

            File.Copy(srcFileName,dstFileName , true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            Debug.Log($"复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            AssetDatabase.Refresh();
        }
    }
}