using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/GameTermsDefine")]
    public class GameTermsDefineOverview : DataOverview<GameTermsDefineOverview, GameTermsDefine>
    {
        public override string TagName => "游戏用语";
    }
    [InitializeOnLoad]
    public class CreateGameTermsDefineOverview
    {
        static CreateGameTermsDefineOverview()
        {
            GameTermsDefineOverview.CreateOverView();
        }
    }
}
