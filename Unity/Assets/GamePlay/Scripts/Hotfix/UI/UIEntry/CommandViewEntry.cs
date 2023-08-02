using GamePlay;
using ZFramework.Core;

namespace UIEntry
{
    /// <summary>
    /// 通用界面调用入口
    /// </summary>
    public static class CommandViewEntry
    {
        public static void OpenMainView()
        {
            var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            uiMgr.Init();
            uiMgr.OpenWindow<MainView>();
        }


        public static void OpenTipsView(string tips)
        {
            var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            uiMgr.Init();
            uiMgr.OpenWindow<PopTipsView>((view) =>
            {
                view.Bind.Get_tips_text.text = tips;
            });
        }
    }
}