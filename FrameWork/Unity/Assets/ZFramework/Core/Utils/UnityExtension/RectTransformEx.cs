using UnityEngine;
using System.Collections;
namespace ZFramework
{
    public static class RectTransformEx
    {
        public static void ToFullScreen(this RectTransform rectTransform)
        {
            rectTransform.offsetMin = rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }
    }
}
