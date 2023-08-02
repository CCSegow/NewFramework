using System.Collections.Generic;
using UnityEngine;

namespace ZFramework.Core.UI
{
    public class ViewLayerManager
    {
        private Stack<ViewLayer> _stackItems = new Stack<ViewLayer>();


        private ViewLayer CurLayer;

        private Dictionary<int, Vector3> _viewShowPositions = new Dictionary<int, Vector3>();
        //打开全屏界面时
        public void OpenView(UIView view)
        {         
            var viewId = view.GetInstanceID();
            _viewShowPositions.TryAdd(viewId,view.transform.position);

            //每次打开全屏界面，就会创建一个layer,每个layer只允许存在一个全屏界面
            //再全屏界面上打开其他类型界面都会记录在该Layer的激活队列中
            //当有新layer时，会把上一个layer中所有界面进行隐藏
            //当layer进行关闭时，会重新激活上一个layer中的所有界面
            if (CurLayer == null)
            {
                NewLayer(null,view);
            }
            else
            {
                if (view.ViewInfo.IsFullView)
                {
                    PushLayer(CurLayer);
                    NewLayer(view,null);
                }
                else
                {
                    CurLayer.ActiveViews.Add(view);
                }
            }
           
        }

        public void CloseView(UIView view)
        {
            Debug.Log($"CloseView {view.ViewInfo.ViewName}");
            if (view.ViewInfo.IsFullView)
            {
                PopLayer();
            }
            else
            {
                CurLayer.ActiveViews.Remove(view);
            }
        }

        private void NewLayer(UIView rootView,UIView subView)
        {
            Debug.Log($"New Layer {_stackItems.Count}");
            CurLayer = new ViewLayer();
            
            CurLayer.RootView = rootView;
            if(subView != null)
                CurLayer.ActiveViews.Add(subView);
        }

        private void PushLayer(ViewLayer layer)
        {
            _stackItems.Push(layer);
            foreach (var view in layer.ActiveViews)
            {
                //隐藏界面
                view.transform.position = new Vector3(-1000, 0, 0);//不做激活处理，直接移除到屏幕外
                view.OnHide();
            }
        }

        private void PopLayer()
        {
            if (_stackItems.Count == 0)
            {
                return;
            }

            //关闭这一层的所有界面
            
            for (int i = CurLayer.ActiveViews.Count - 1; i >= 0; i--)
            {
                var view = CurLayer.ActiveViews[i];
                GameManager.Ins.GetGameComponent<UIManagerComponent>().CloseWindow(view.ViewInfo.ViewName);
            }
            

            CurLayer = _stackItems.Pop();
            //激活这一层的所有界面
            foreach (var view in CurLayer.ActiveViews)
            {
                //view.transform.position = new Vector3(0, 0, 0);
                var viewId = view.GetInstanceID();
                if (_viewShowPositions.TryGetValue(viewId, out var pos))
                {
                    view.transform.position = pos;//还原位置
                    _viewShowPositions.Remove(viewId);
                }

                view.OnResume();
            }
        }
    }

    public class ViewLayer
    {
        public UIView RootView;//全屏界面 （初始Layer的根界面为空）
        public List<UIView> ActiveViews = new List<UIView>();//不包括全屏界面
    }
}