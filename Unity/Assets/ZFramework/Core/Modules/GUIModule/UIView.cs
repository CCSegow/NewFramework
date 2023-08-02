using System;
using UnityEngine;

namespace ZFramework.Core
{
    //Bind文件 和 View文件分开是为了重新导出Bind文件时不影响逻辑代码
    
    public abstract class UIBind : MonoBehaviour
    {
        
    }

    public abstract class UIView:MonoBehaviour
    {
        public UIViewInfo ViewInfo { get; set; }

        /// <summary>
        /// 关闭此界面
        /// </summary>
        public void CloseThis()
        {
            GameManager.Ins.GetGameComponent<UIManagerComponent>().CloseWindow(ViewInfo.ViewName);
        }

        /// <summary>
        /// 界面加载完毕时
        /// </summary>
        public virtual void OnLoadFinish()
        {
            
        }

        /// <summary>
        /// 界面打开时
        /// </summary>
        public virtual void OnOpen()
        {
        }

        /// <summary>
        /// 界面关闭时
        /// </summary>
        public virtual void OnClose()
        {
        }

        /// <summary>
        /// 界面销毁时
        /// </summary>
        public virtual void OnDestroyView()
        {
        }

        /// <summary>
        /// 隐藏时：被其他界面遮挡而隐藏
        /// </summary>
        public virtual void OnHide()
        {
        }

        /// <summary>
        /// 重新激活时：遮挡界面被关闭了，重新显示此界面时
        /// </summary>
        public virtual void OnResume()
        {
            
        }
    }
}