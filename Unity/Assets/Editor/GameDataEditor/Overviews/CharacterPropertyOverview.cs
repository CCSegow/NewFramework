using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/CharacterPropertyData")]
    public class CharacterPropertyOverview : DataOverview<CharacterPropertyOverview, CharacterPorpertyData>
    {
        public override string TagName => "职业（种族）类型";
    }
    [InitializeOnLoad]
    public class CreateCharacterPropertyOverview
    {
        static CreateCharacterPropertyOverview()
        {
            CharacterPropertyOverview.CreateOverView();
        }
    }
}
