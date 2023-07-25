using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/UIViewData")]
    public class UIViewOverview:DataOverview<UIViewOverview,UIViewData>
    {
     
        [OnValueChanged("CreateUIData"),LabelText("拖放UI预制体到此处 创建该界面的配置")]
        public GameObject UIObject;

        [FolderPath,LabelText("UI总配置导出路径"),OnValueChanged("OnUpdateView")]
        public string ExportPath;

        public UIViewSetting UIViewSetting;
        
        [Button("更新UI配置",ButtonSizes.Large)]
        void CreateUIViewSetting()
        {
            var ViewItems = new UIViewInfo[AllDatas.Length];
            for (int i = 0; i < AllDatas.Length; i++)
            {
                ViewItems[i] = AllDatas[i].UIViewInfo;
            }

            UIViewSetting.ViewItems = ViewItems;
            EditorUtility.SetDirty(UIViewSetting);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public override string TagName => "UI界面";

        protected override void OnUpdateView()
        {
            UIViewSetting = Get;
        }

        UIViewSetting Get
        {
            get
            {
                if (string.IsNullOrEmpty(ExportPath))
                {
                    return null;
                }

                var path = ExportPath + "/UIViewSetting.asset";
                var asset = AssetDatabase.LoadAssetAtPath<UIViewSetting>(path);
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance<UIViewSetting>();
                    AssetDatabase.CreateAsset(asset, path);
                    AssetDatabase.SaveAssets();
                }

                return asset;
            }
        }

        void CreateUIData()
        {
            if(UIObject == null)
                return;
            if (IsExits(UIObject.name))
            {
                //Debug.Log($"已存在 {UIObject.name}");
                return;
            }
            //Debug.Log($"创建 {UIObject.name}");
            var data = CreateNew(UIObject.name);
            data.UIPrefab = UIObject;
            data.UpdateUIViewInfo();
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
    
    [InitializeOnLoad]
    public class CreateUIViewOverview
    {
        static CreateUIViewOverview()
        {
            UIViewOverview.CreateOverView();
        }
    }
}