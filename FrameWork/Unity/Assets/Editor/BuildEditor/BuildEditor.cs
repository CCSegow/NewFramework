using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace BuildTool
{
	public class BuildEditor : OdinEditorWindow
	{		
		[TabGroup("打包配置管理"),InlineEditor(inlineEditorMode: InlineEditorModes.FullEditor,objectFieldMode: InlineEditorObjectFieldModes.Hidden)]
		public BuildConfig BuildConfig;		
		
		[TabGroup("资源服务器管理"),InlineEditor(inlineEditorMode: InlineEditorModes.FullEditor,objectFieldMode: InlineEditorObjectFieldModes.Hidden)]
		public ServerConfig ServerConfig;

		[MenuItem("Tools/打包工具")]
		public static void ShowWindow()
		{
			var window = GetWindow(typeof(BuildEditor));
			window.titleContent = new GUIContent("打包工具");
			window.minSize = new Vector2(800, 600);
		}

        protected override void OnEnable()
        {
	        BuildConfig = BuildConfig.Get;
	        ServerConfig = ServerConfig.Get;
        }

        [Button]
        public void TestPath()
        {
	        Debug.Log($"{PathUtil.ProjectPath}");
        }
	}
}
