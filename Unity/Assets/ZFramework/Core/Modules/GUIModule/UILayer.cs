using UnityEngine;
using System.Collections;
namespace ZFramework.Core {
    public enum E_UILayer
    {
        Bg,
        View,
        HUD,//战斗中的界面层级
        Pop,
        Tip,
        Top
    }

    public enum E_ShowFrequency
    {
        One,//仅显示一次
        Normal,//有可能打开
        Offent,//经常打开
    }
}
