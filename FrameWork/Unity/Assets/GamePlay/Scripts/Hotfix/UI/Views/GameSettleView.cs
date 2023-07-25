using System.Collections;
using UnityEngine.SceneManagement;
using ZFramework.Core;
namespace GamePlay
{
	public class GameSettleView:UIView
	{
		public GameSettleView_Bind Bind;
        public override void OnLoadFinish()
        {
            Bind.Get_rechallenge_btn.onClick.AddListener(OnClickReChallenge);
            Bind.Get_back2home_btn.onClick.AddListener(OnClickBack2Home);
        }
        private void OnClickReChallenge()
        {
            var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            uiMgr.CloseWindow<GameSettleView>();
            GamePlayData.Instance.EventTriggerMgrEx.ReChallenge();
            GamePlayData.Instance.CharacterMgrEx.ReChallenge();
            GamePlayData.Instance.MapBlockMgrEx.ReChallenge();
            uiMgr.CloseWindow<LoadingView>();
        }
        private void OnClickBack2Home()
        {
            var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            SceneManager.UnloadSceneAsync("RunDemo001");
            uiMgr.CloseWindow<GameSettleView>();
            uiMgr.OpenWindow<StartGameView>();
        }
    }
}
