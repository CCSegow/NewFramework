#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class EditorUtils 
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="localPath"></param>
    public static T CreateScriptableObject<T>(string localPath,string name) where T : ScriptableObject {
        if (string.IsNullOrEmpty( name)) {
            Debug.LogError("新建资源名字不能为空");
            return null;
        }

        T newItem = ScriptableObject.CreateInstance<T>();

        if (localPath == "")
        {
            localPath = $"Assets/Data/";
        }
        else {
            localPath = $"Assets/Data/{localPath}";
        }
        AssetDatabase.CreateAsset(newItem, $"{localPath}/{name}.asset");
        AssetDatabase.SaveAssets();
        return newItem;
    }
}
#endif