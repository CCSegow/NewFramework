using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using ZFramework.Core;
using Sirenix.Utilities;

namespace ZFramework.Core.Editor
{
    public class GameDataManagerEditor : OdinMenuEditorWindow
    {
        private bool treeRebuild;


        [MenuItem("YimiFramework/The Game Manager")]
        public static void OpenWindow()
        {
            GetWindow<GameDataManagerEditor>().Show();
        }

        protected override void Initialize()
        {
        }

        protected override void OnGUI()
        {
            if (treeRebuild && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                treeRebuild = false;
            }

            SirenixEditorGUI.Title("游戏数据管理", "统一管理游戏中各个模块的数据", TextAlignment.Center, true);
            base.OnGUI();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            LoadAllData(tree);
            return tree;
        }

        void LoadAllData(OdinMenuTree tree)
        {
            var AllDatas = AssetDatabase.FindAssets($"t:{typeof(ScriptableObject)}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            foreach (var data in AllDatas)
            {
                if (data.name == "LubanExportConfig")
                {
                    Debug.Log($"{data.name},{data.GetType()}");
                    Debug.LogWarning(data is I_DataOverview);
                    
                }

             
                if (data is I_DataOverview overViewData)
                {
                    overViewData.UpdateOverview();
                    AddItem(tree,overViewData);
                }                        
            }
        }
        void AddItem(OdinMenuTree tree, I_DataOverview overView)
        {
            overView.OnCreateNewData = CreateNewData;
            var tag = overView.TagName;
            tree.Add(tag, overView);
            tree.AddAllAssetsAtPath(tag, overView.AssetPath, overView.GetDataType);
        }

        private void CreateNewData()
        {
            treeRebuild = true;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delect Select")))
                {
                    if (EditorUtility.DisplayDialog("Warm", $"将删除 {selected.Name}", "Yes", "No"))
                    {
                        if (selected.Value != null)
                        {
                            string path = AssetDatabase.GetAssetPath(selected.Value as Object);
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.SaveAssets();
                        }

                        treeRebuild = true;
                    }
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Load All")))
                {
                    
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}