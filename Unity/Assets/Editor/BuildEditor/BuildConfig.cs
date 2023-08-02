using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace BuildTool
{
    //[CreateAssetMenu(menuName = "打包/打包配置",fileName = "默认打包配置")]
    public class BuildConfig:ScriptableObject
    {     
        private const string AssetPath = "Assets/Editor/BuildEditor/BuildConfig.asset";
        public static BuildConfig Get
        {
            get
            {
                var asset = AssetDatabase.LoadAssetAtPath<BuildConfig>(AssetPath);
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance<BuildConfig>();
                    AssetDatabase.CreateAsset(asset, AssetPath);
                    AssetDatabase.SaveAssets();
                }

                return asset;
            }
        }
        
        [System.Serializable]
        public class BuildSetting
        {            
            [FoldoutGroup("$Name"),LabelText("配置备注")]
            public string Name;
            
            [FoldoutGroup("$Name")]
            [HorizontalGroup("$Name/Root",Width = 0.3f),BoxGroup("$Name/Root/热更dll生成", centerLabel:true),HideLabel,InlineProperty]
            public HotfixDllBuildSetting HotfixConfig;
            
            [BoxGroup("$Name/Root/资源构建", centerLabel:true),HideLabel,InlineProperty]
            public BundleBuildSetting BundleBuildSetting;
            
            

            [BoxGroup("$Name/Root/应用构建", centerLabel:true),HideLabel,InlineProperty]
            public AppBuildSetting AppBuildSetting;
            
            [FoldoutGroup("$Name"),Button("执行构建",ButtonSizes.Large),GUIColor(0,1,0.1f)]
            public void Build()
            {

                HotfixConfig.Build();
                BundleBuildSetting.Build();
                AppBuildSetting.Build();
            }

            
        }

        public BuildSetting[] BuildSettings;


        
    }
}