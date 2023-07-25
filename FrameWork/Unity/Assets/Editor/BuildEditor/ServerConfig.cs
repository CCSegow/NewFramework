using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

namespace BuildTool
{
    [System.Serializable]
    //[CreateAssetMenu(fileName = "ServerConfig", menuName = "打包/服务器配置")]
    public class ServerConfig : ScriptableObject
    {
        private const string AssetPath = "Assets/Editor/BuildEditor/ServerConfig.asset";

        public static ServerConfig Get
        {
            get
            {
                var asset = AssetDatabase.LoadAssetAtPath<ServerConfig>(AssetPath);
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance<ServerConfig>();
                    AssetDatabase.CreateAsset(asset, AssetPath);
                    AssetDatabase.SaveAssets();
                }

                return asset;
            }
        }

        [LabelText("服务器列表")] public SeverItem[] ServerList;

        [System.Serializable]
        public class Bundle
        {
            public string BundleName;
            public string BundleVersion;
        }

        [System.Serializable]
        public class SeverItem
        {
            [LabelText("服务器名")] public string ServerName = "Server1";
            [LabelText("备注")] public string Describe = "服务器1";
            public string Host = "127.0.0.1";
            public string Port = "83";

            [FolderPath] public string BundlePath = "";

            [LabelText("本地服务器")] public bool isLocalServer;
            public Bundle[] Bundles;

            [ValueDropdown("GetBundles")] public string ActiveBundle;

            IEnumerable GetBundles()
            {
                return Bundles.Select(x => new ValueDropdownItem(x.BundleName, x.BundleName));
            }

            [Button("更新服务器配置"), GUIColor(1, 1, 0)]
            void UpdateServerConfig()
            {
                if (isLocalServer)
                {
                    //UpdateLocalServerConfig();
                }
                else
                {
                    UpdateRemoteServerConfig();
                }
            }

            Bundle GetActiveBundle()
            {
                foreach (var bundle in Bundles)
                {
                    if (bundle.BundleName == ActiveBundle)
                    {
                        return bundle;
                    }
                }

                return null;
            }

/*
		void UpdateLocalServerConfig()
		{
			var bundle = GetActiveBundle();
			if (bundle == null)
			{
				Debug.LogError("请选择需要更新的Bundle");
				return;
			}

			//本地测试用
			AssetServerData serverData = new AssetServerData();
			serverData.ServerName = ServerName;
			serverData.PackageName = bundle.BundleName;
			serverData.Version = bundle.BundleVersion;
			
			serverData.DefaultUrl = $"http://{Host}:{Port}/{BundlePath}";
			serverData.FallbackUrl = $"http://{Host}:{Port}/{BundlePath}";//TODO 暂时不处理

			var json = JsonHelper.ToJson(serverData);
			Debug.Log(json);

			var path = Path.Combine( GlobalTool.GetGitProjectPath,$"TestServer/wwwroot/{ServerName}.json");
			Debug.Log($"write to :{path}");
			File.WriteAllText(path,json);
		}
*/
            void UpdateRemoteServerConfig()
            {
            }
        }
    }
}