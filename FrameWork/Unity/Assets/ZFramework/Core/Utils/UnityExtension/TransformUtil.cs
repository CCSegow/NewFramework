using System;
using UnityEngine;
using System.Collections;
namespace ZFramework {
    public static class TransformUtil
    {
        /// <summary>
        /// 销毁所有子对象
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyAllChildren(this Transform transform) {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        
        /// <summary>
        /// 遍历所有子对象
        /// </summary>
        /// <param name="transform">根对象</param>
        /// <param name="onEach">每个子对象的处理</param>
        public static void Foreach(this Transform transform,Action<Transform> onEach) {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                onEach(child);
                Foreach(child,onEach);
            }
        }
        
        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="transform">根对象</param>
        /// <param name="name">子对象名字</param>
        /// <param name="isAll">所有同名子对象</param>
        /// <param name="onEach">对每个子对象都进行返回处理</param>
        public static void Find(this Transform transform,string name,bool isAll,Action<Transform> onEach) {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                if (child.name == name)
                {                    
                    onEach(child);
                    if(!isAll)
                        return;
                }                
                Find(child,name,isAll,onEach);
            }
        }
    }

}
