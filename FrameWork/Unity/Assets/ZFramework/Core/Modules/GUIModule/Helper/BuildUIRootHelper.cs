using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System;

namespace ZFramework.Core.Util {
    public class BuildUIRootHelper : MonoBehaviour
    {
        [InlineEditor]
        public UIViewSetting UIViewSetting;

        [Button]
        public void RebuildRoot() {

            transform.DestroyAllChildren();

            var layerType = typeof(E_UILayer);
            var names = Enum.GetNames(layerType);
            int layer = LayerMask.NameToLayer("UI");
            foreach (var name in names)
            {
                GameObject obj = new GameObject(name, typeof(RectTransform));
                obj.layer = layer;
                var rectTsf = obj.transform as RectTransform;
                rectTsf.SetParent(transform);
                rectTsf.ToFullScreen();
            }
            gameObject.name = "UIRoot";
        }
#if UNITY_EDITOR
        [HideIf("UIViewSetting"), Button]        
        public void CreateUIViewSetting() {

            UIViewSetting = EditorUtils.CreateScriptableObject<UIViewSetting>("UI", "UIViewSetting");

        }
#endif
    }

}
