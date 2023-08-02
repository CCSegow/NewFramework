using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System.Linq;
using System;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    public interface I_DataOverview
    {
        public void UpdateOverview();
        Action OnCreateNewData { get; set; }

        public string AssetPath { get; }

        public Type GetDataType { get; }
        
        public string TagName { get; }
    }

 
    public class DataOverview<T,U>  :  GlobalConfig<T>,I_DataOverview  
        where T : DataOverview<T, U>,new()
        where U : ScriptableObject
    {

        [ReadOnly]
        [ListDrawerSettings(ShowFoldout = true)]
        public U[] AllDatas;
        public Action OnCreateNewData { get; set; }
        public Type GetDataType => typeof(U);
        public virtual  string TagName { get; }

        public static void CreateOverView()
        {
            //Debug.Log($"{typeof(T)}");
            //默认创建
            var path = ConfigAttribute.AssetPath;
             //Debug.Log($"{path}");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            GlobalConfigUtility<T>.GetInstance(path);
        }


        [LabelWidth(100)]
        [PropertyOrder(-1)]

        [GUIColor(0.7f, 0.7f, 1f)]
        [Button(ButtonStyle.Box)]
        public U CreateNew(string nameForNew)
        {
            if (nameForNew == "")
            {
                return null;
            }

            U newItem = ScriptableObject.CreateInstance<U>();

            var path = AssetPath;
            AssetDatabase.CreateAsset(newItem, $"{path}{nameForNew}.asset");
            AssetDatabase.SaveAssets();

            OnCreateNewData?.Invoke();
            return newItem;
        }

        public bool IsExits(string assetName)
        {
            var path = $"{AssetPath}{assetName}.asset";
            Debug.Log(path);
            return File.Exists(path);
        }


        [Button(ButtonSizes.Medium,DrawResult = false), PropertyOrder(-1),GUIColor(0.7f,1f,0.7f,1f)]
        public void UpdateOverview()
        {
            this.AllDatas = AssetDatabase.FindAssets($"t:{typeof(U)}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<U>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            OnUpdateView();
        }

        protected virtual void OnUpdateView()
        {
            
        }

        public string AssetPath {
            get {
                var type = this.GetType();
                var attr = type.GetAttribute<GlobalConfigAttribute>();
                if (attr.AssetPath == "")
                    return "Assets/";
                return attr.AssetPath;
            }
        }
    }

}
