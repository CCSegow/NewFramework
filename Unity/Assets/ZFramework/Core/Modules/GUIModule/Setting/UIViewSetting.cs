using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace ZFramework.Core {
    [System.Serializable]
    public struct UIViewInfo
    {
        public string AssetURL;
        public string ViewName;
        public bool IsFullView;//全屏界面，会隐藏它下面的其他界面
        public E_UILayer Layer;
        public E_ShowFrequency ShowFrequency;
        
// #if UNITY_EDITOR
//         [Button,GUIColor(0,1,0),ButtonGroup]
//         void OpenView()
//         {
//             GameManager.Ins.GetGameComponent<UIManagerComponent>().OpenWindow(ViewName);
//         }
//
//         [Button,GUIColor(1,0,0),ButtonGroup]
//         void CloseView()
//         {
//             GameManager.Ins.GetGameComponent<UIManagerComponent>().CloseWindow(ViewName);
//         }
// #endif
    }
    
    public class UIViewSetting : ScriptableObject
    {
        [ReadOnly]
        public UIViewInfo[] ViewItems;
    }

}
