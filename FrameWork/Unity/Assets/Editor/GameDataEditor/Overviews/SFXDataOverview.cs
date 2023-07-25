using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/SFXData")]
    public class SFXDataOverview : DataOverview<SFXDataOverview, SFXData>
    {
        public override string TagName => "音效";
    }
    [InitializeOnLoad]
    public class CreateSFXDataOverview
    {
        static CreateSFXDataOverview()
        {
            SFXDataOverview.CreateOverView();
        }
    }
}
