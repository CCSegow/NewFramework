using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/BuffData")]
    public class BuffOverview : DataOverview<BuffOverview, BuffData>
    {
        public override string TagName => "Buff";
    }   
    
    [InitializeOnLoad]
    public class CreateBuffOverview
    {
        static CreateBuffOverview()
        {
            BuffOverview.CreateOverView();
        }
    }
}