using UnityEngine;
using System.Collections;
using UnityEditor;
namespace ZFramework 
{
    public class UIEditorHelper 
    {
        [MenuItem("GameObject/Zero Framework/UI Root",false,11)]
        static void CreateUIRoot(MenuCommand menuCommand) {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/UIRoot.prefab");
            
            // Create a custom game object
            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}