using UnityEngine;
using System.Collections;
using UnityEditor;
using Sirenix.OdinInspector;

public class DataToolBar<T> where T :ScriptableObject
{

    private string _path;

    [LabelWidth(100)]
    [PropertyOrder(-1)]
    [BoxGroup("CreateNew")]
    [HorizontalGroup("CreateNew/Horizontal")]
    public string nameForNew;


    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(0.7f, 0.7f, 1f)]
    [Button]
    public void CreateNew()
    {
        if (nameForNew == "")
        {
            return;
        }

        T newItem = ScriptableObject.CreateInstance<T>();
        //newItem.name = "New " + typeof(T).ToString();

        if (_path == "")
        {
            _path = "Assets/";
        }
        AssetDatabase.CreateAsset(newItem, $"{_path}\\{nameForNew}.asset");
        AssetDatabase.SaveAssets();
    }

    public void SetPath(string path)
    {
        this._path = path;
    }
}
