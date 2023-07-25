using UnityEngine;
using ZFramework.Core;
namespace GamePlay
{
	public class MainView:UIView
	{
		public MainView_Bind Bind;

		public override void OnLoadFinish()
		{
			Bind.Get_start_btn.onClick.AddListener(OnClickStart);
		}

		void OnClickStart()
		{
			Debug.Log("OnClick Start");

			//LoadScene();
			PrintConfig();
		}

		void PrintConfig()
		{
			TestData.GetData("item_tbitem");
		}

		void LoadScene()
		{
			var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
			uiMgr.OpenWindow<LoadingView>((view) =>
			{
				uiMgr.CloseWindow<MainView>();
				var sceneMgr = GameManager.Ins.GetGameComponent<SceneManagerComponent>();
			
				var scenePath = "Assets/Bundles/Scenes/level0.unity";
				sceneMgr.LoadScene(scenePath, true);
				
			});
		}




	}
}
