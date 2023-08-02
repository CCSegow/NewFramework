using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/SkillData")]
    public class SkillOverview : DataOverview<SkillOverview, SkillData>
    {
        public override string TagName => "技能";
    }
    [InitializeOnLoad]
    public class CreateSkillOverview
    {
        static CreateSkillOverview()
        {
            SkillOverview.CreateOverView();
        }
    }
}
