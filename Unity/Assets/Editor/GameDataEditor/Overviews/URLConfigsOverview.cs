using Sirenix.Utilities;
using UnityEditor;
using ZFramework.Core;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/URLConfigs")]
    public class URLConfigsOverview : DataOverview<URLConfigsOverview, URLConfigs>
    {
        public override string TagName => "URL";
    }
    
    [InitializeOnLoad]
    public class CreateURLConfigsOverview
    {
        static CreateURLConfigsOverview()
        {
            URLConfigsOverview.CreateOverView();
        }
    }
}