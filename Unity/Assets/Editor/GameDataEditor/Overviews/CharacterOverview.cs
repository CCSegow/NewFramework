using Sirenix.Utilities;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/CharacterData")]
    public class CharacterOverview : DataOverview<CharacterOverview,CharacterData>
    {
        public override string TagName => "角色";
    }
   
}
