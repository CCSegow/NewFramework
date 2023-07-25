using Sirenix.Utilities;
using UnityEditor;

namespace ZFramework.Core.Editor
{
    //[("定义各种常用的类型，如护甲类型，武器类型")]
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/TypeDefine")]
    public class TypeDefineOverview : DataOverview<TypeDefineOverview, TypeDefine>
    {
        public override string TagName => "类型定义";
    }
    [InitializeOnLoad]
    public class CreateTypeDefineOverview
    {
        static CreateTypeDefineOverview()
        {
            TypeDefineOverview.CreateOverView();
        }
    }
}
