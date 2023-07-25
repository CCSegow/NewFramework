using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/VFXData")]
    public class VFXDataOverview : DataOverview<VFXDataOverview, VFXData>
    {
        public override string TagName => "特效";
    }
    [InitializeOnLoad]
    public class CreateVFXDataOverview
    {
        static CreateVFXDataOverview()
        {
            VFXDataOverview.CreateOverView();
        }
    }
}
