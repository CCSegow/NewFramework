using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace ZFramework.Core.Editor
{
    [GlobalConfig("Assets/Editor/GameDataEditor/Data/ItemData")]
    public class ItemOverview : DataOverview<ItemOverview, ItemData>
    {
        public override string TagName => "道具";
    }
    [InitializeOnLoad]
    public class CreateItemOverview
    {
        static CreateItemOverview()
        {
            ItemOverview.CreateOverView();
        }
    }
}