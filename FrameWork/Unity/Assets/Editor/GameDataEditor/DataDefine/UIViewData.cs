using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ZFramework.Core.Editor
{
    public class UIViewData: ScriptableObject
    {
        [OnValueChanged("UpdateUIViewInfo")]
        public GameObject UIPrefab;
        [HideLabel,BoxGroup]
        public UIViewInfo UIViewInfo;

        public void UpdateUIViewInfo()
        {
            if (UIPrefab == null)
            {
                return;
            }

            UIViewInfo.AssetURL = AssetDatabase.GetAssetPath(UIPrefab);
            UIViewInfo.ViewName = UIPrefab.name;
        }

        [Button("导出界面绑定代码")]
        void GenerateBindCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
@"using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFramework.Core;");

            sb.AppendLine("\nnamespace GamePlay");
            sb.AppendLine("{");
            
            sb.AppendLine($"\tpublic class {UIViewInfo.ViewName}_Bind:UIBind");
            sb.AppendLine("\t{");
            UIPrefab.transform.Foreach((obj) =>
            {
                //Debug.Log(obj.name);
                var objectName = obj.name;
                if (!objectName.StartsWith('_'))
                {
                    return;
                }

                string valueType = "RectTransform";
                if (objectName.EndsWith("_textPro"))
                {
                    valueType = "TextMeshProUGUI";
                }
                else if (objectName.EndsWith("_text"))
                {
                    valueType = "Text";
                }
                else if (objectName.EndsWith("_btn"))
                {
                    valueType = "Button";
                }
                else if (objectName.EndsWith("_img"))
                {
                    valueType = "Image";
                }

                sb.AppendLine($"\t\t[SerializeField]");
                sb.AppendLine($"\t\tprivate {valueType} {objectName};");
                sb.AppendLine($"\t\tpublic {valueType} Get{objectName} => {objectName};");
            });
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            
            Debug.Log(sb.ToString());
            
            var codePath = "Assets/GamePlay/Scripts/Hotfix/UI/Binds/"+$"{UIViewInfo.ViewName}_Bind.cs";
            File.WriteAllText(codePath,sb.ToString());
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [Button("导出界面代码")]
        void GenerateViewCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                @"
using ZFramework.Core;");

            sb.AppendLine("\nnamespace GamePlay");
            sb.AppendLine("{");
            
            sb.AppendLine($"\tpublic class {UIViewInfo.ViewName}:UIView");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tpublic {UIViewInfo.ViewName}_Bind Bind;");
            sb.AppendLine("\t}");
            
            sb.AppendLine("}");
            
            var codePath = "Assets/GamePlay/Scripts/Hotfix/UI/Views/"+$"{UIViewInfo.ViewName}.cs";
            File.WriteAllText(codePath,sb.ToString());
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [Button]
        void AttachScript()
        {
            Assembly assembly = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
            
            
            //添加Bind组件
            var bindTypeName = $"GamePlay.{UIViewInfo.ViewName}_Bind";
            var bindComponent = AddComponent(bindTypeName,assembly);
            
            var bindType = assembly.GetType(bindTypeName);
            var fields = bindType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);            

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                
                UIPrefab.transform.Find($"{field.Name}",false, (child) =>
                { 
                    var value = child.GetComponent(field.FieldType);
                    field.SetValue(bindComponent,value);
                });                
            }
            
            //添加View组件
            var viewTypeName = $"GamePlay.{UIViewInfo.ViewName}";
            var viewComponent = AddComponent(viewTypeName,assembly);
            var viewType = assembly.GetType(viewTypeName);

            var bindField = viewType.GetField("Bind", BindingFlags.Instance | BindingFlags.Public);
    
            bindField.SetValue(viewComponent,bindComponent);
            
            EditorUtility.SetDirty(bindComponent);
            EditorUtility.SetDirty(viewComponent);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Component AddComponent(string componentName,Assembly assembly)
        {
            var type = assembly.GetType(componentName);
            if (type == null)
            {
                Debug.LogError("Can not find ");
            }            
            
            var instance = UIPrefab.GetComponent(type);
            if (instance == null)
            {
                instance = UIPrefab.AddComponent(type);
            }

            return instance;
        }
    }
}