using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/WeaponData")]
    public class WeaponOverview : DataOverview<WeaponOverview, WeaponData>
    {
        public override string TagName => "武器";
    }
    
    [InitializeOnLoad]
    public class CreateWeaponOverview
    {
        static CreateWeaponOverview()
        {
            WeaponOverview.CreateOverView();
        }
    }
}
