using ZFramework.Core;
using UnityEngine;
namespace GamePlay
{
	public class StartGameView:UIView
	{
		public StartGameView_Bind Bind;
        private float tipShowTime;


        public override void OnLoadFinish()
        {
            Bind.Get_start_btn.onClick.AddListener(OnClickStart);
            Bind.Get_tip_text.text = "";
            RefreshView();
        }
        private void Update()
        {
            if(tipShowTime>0f)
            {
                tipShowTime -= Time.deltaTime;
                if(tipShowTime <= 0f)
                {
                    Bind.Get_tip_text.text = "";
                }
            }
        }
        void OnClickStart()
        {
            if(!GameDataOK())
            {
                tipShowTime = 1f;
                Bind.Get_tip_text.text = $"关卡或地图ID存在错误";
                return;
            }
            var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            uiMgr.OpenWindow<LoadingView>((view) =>
            {
                uiMgr.CloseWindow<StartGameView>();
            });
            var sceneMgr = GameManager.Ins.GetGameComponent<SceneManagerComponent>();

            var scenePath = "Assets/Bundles/Scenes/RunDemo001.unity";
            sceneMgr.LoadScene(scenePath, true);
        }
        public override void OnClose()
        {
            base.OnClose();
            this.gameObject.SetActive(false);
        }
        public override void RefreshView()
        {
            Bind.Get_schedule_text.text = $"当前关卡ID：{GamePlayData.Instance.CurMapID}";
            Bind.Get_game_data_text.text = $"当前角色ID：{GamePlayData.Instance.CurCharactersID}";
        }
        private bool GameDataOK()
        {
            var mapID = TestData.GetMapData(GamePlayData.Instance.CurMapID);
            var characterID = TestData.GetCharacterData(GamePlayData.Instance.CurCharactersID);
            return mapID != null && characterID != null;
        }
    }
}
