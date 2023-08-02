/* ---------------------------------
 * 负责界面加载，卸载
 * UI层级管理
 * 只提供界面的打开，关闭接口
 * 其他界面相关操作逻辑下放到各自的UIEntry中处理
 * ---------------------------------
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using ZFramework.Core.UI;

namespace ZFramework.Core
{
    public class UIManagerComponent : ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(UIManagerComponent);
        public string UISettingID = "Assets/Bundles/Configs/UI/UIViewSetting.asset";

        UIManagerComponent()
        {
        }

        //private RectTransform _root;
        private bool _isInit;

        private Dictionary<E_UILayer, RectTransform> _layers;

        private Dictionary<string, UIViewInfo> _viewInfos;

        /// <summary>
        /// 当前打开的界面
        /// </summary>
        private Dictionary<string, UIView> _activeViews;

        /// <summary>
        /// 关闭的界面
        /// </summary>
        private Dictionary<string, UIView> _closeViews;
        
        class OpenViewCommand
        {
            public string ViewName;
            public Action<UIView> OnOpen;
        }

        private Queue<OpenViewCommand> _openViewCommands;

        struct DelayCloseView
        {
            public float DelayTime;
            public float Timer;

            public string viewName;
        }

        private List<DelayCloseView> _delayDestroyList;
        private ViewLayerManager _viewLayerManager;

        private I_AssetServant _uiServant;

        protected override void OnInit()
        {
            _layers = new Dictionary<E_UILayer, RectTransform>();
            _activeViews = new Dictionary<string, UIView>();
            _viewInfos = new Dictionary<string, UIViewInfo>();

            _openViewCommands = new Queue<OpenViewCommand>();
            _closeViews = new Dictionary<string, UIView>();
            _delayDestroyList = new List<DelayCloseView>();

            _viewLayerManager = new ViewLayerManager();
            LoadUIRoot();
        }

        async void LoadUIRoot()
        {
            var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
            _uiServant = assetComponent.GetServant(); //持久服务，自己控制释放时机

            var temp_servant = assetComponent.GetServant(); // 一次性服务，用完即删

            //获取ui配置
            var viewSetting = await temp_servant.GetAssetAsyncTask<UIViewSetting>(UISettingID);
            foreach (var viewInfo in viewSetting.ViewItems)
            {
                _viewInfos.Add(viewInfo.ViewName, viewInfo);
                Debug.Log($"add {viewInfo.ViewName}:{viewInfo.AssetURL}");
            }

            // 获取ui根目录结构
            var uiRoot =
                GameObject.Instantiate(
                    await temp_servant.GetAssetAsyncTask<GameObject>("Assets/Bundles/Prefabs/UI/UIRoot.prefab"));
            GameObject.DontDestroyOnLoad(uiRoot);
            var layers = uiRoot.transform.Find("Layers");
            var Layers_transform = layers.GetComponent<RectTransform>();

            //初始化UI层级
            var layerType = typeof(E_UILayer);
            var names = Enum.GetNames(layerType);
            
            foreach (var name in names)
            {
                var layer = Layers_transform.Find(name);
                if (layer != null)
                {
                    _layers.Add((E_UILayer)Enum.Parse(layerType, name), layer as RectTransform);
                }
            }

            temp_servant.DisposeAll();
            _isInit = true;
        }

        private void Update()
        {
            if (!_isInit)
            {
                return;
            }

            //检查是否有命令需要执行
            CheckCommand();

            //销毁界面
            DestroyViews();
        }

        void CheckCommand()
        {
            if (_openViewCommands.Count == 0)
            {
                return;
            }

            var command = _openViewCommands.Dequeue();
            //LoadView


            if (_closeViews.TryGetValue(command.ViewName, out var view))
            {
                //从延迟销毁队列中移除
                for (int i = _delayDestroyList.Count - 1; i >= 0; i--)
                {
                    if (_delayDestroyList[i].viewName == command.ViewName)
                    {
                        _delayDestroyList.RemoveAt(i);
                        break;
                    }
                }

                _closeViews.Remove(view.ViewInfo.ViewName);
                //重新打开界面
                OnOpenView(view, command.OnOpen);
                return;
            }

            //还没打开过的界面，或者已经卸载的界面
            LoadView(command);
        }

        void DestroyViews()
        {
            if (_delayDestroyList.Count == 0)
                return;
            for (int i = 0; i < _delayDestroyList.Count; i++)
            {
                var item = _delayDestroyList[i];
                item.Timer += Time.deltaTime;
                _delayDestroyList[i] = item;
                if (item.Timer >= item.DelayTime)
                {
                    //销毁界面
                    if (!_closeViews.TryGetValue(item.viewName, out var view))
                    {
                        Debug.LogError($"销毁界面 {item.viewName}时，界面已经不在关闭列表中了");
                        continue;
                    }

                    DestroyView(view);
                    _delayDestroyList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="command"></param>
        async void LoadView(OpenViewCommand command)
        {
            var viewInfo = GetViewInfo(command.ViewName);
            if (string.IsNullOrEmpty(viewInfo.AssetURL))
            {
                Debug.LogError($"{command.ViewName}的界面配置为空，请更新 UViewSetting");
                return;
            }

            var viewAsset = await _uiServant.GetAssetAsyncTask<GameObject>(viewInfo.AssetURL);
            if (viewAsset == null)
            {
                return;
            }

            var parent = GetLayerParent(viewInfo.Layer);
            var view = GameObject.Instantiate(viewAsset, parent).GetComponent<UIView>();
            view.ViewInfo = viewInfo;

            view.OnLoadFinish();
            OnOpenView(view, command.OnOpen);
        }


        /// <summary>
        /// 打开界面时的处理
        /// </summary>
        /// <param name="view"></param>
        /// <param name="callBack"></param>
        void OnOpenView(UIView view, Action<UIView> callBack)
        {
            view.gameObject.SetActive(true);
            view.OnOpen();
            Debug.LogWarning($"激活界面 {view.ViewInfo.ViewName}");

            _activeViews.Add(view.ViewInfo.ViewName, view);
            _viewLayerManager.OpenView(view);
            callBack?.Invoke(view);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="callBack"></param>
        /// <typeparam name="T"></typeparam>
        public void OpenWindow<T>(Action<T> callBack = null) where T : UIView
        {
            var viewName = typeof(T).Name;

            if (_activeViews.ContainsKey(viewName))
            {
                Debug.Log($"界面已经打开 {viewName}"); // 根据需求，把界面显示到同层级的最顶层
                return;
            }

            _openViewCommands.Enqueue(new OpenViewCommand()
            {
                ViewName = viewName,
                OnOpen = (v) => { callBack?.Invoke(v as T); }
            });
        }

        /// <summary>
        /// 打开界面，（此接口一般给AOT层使用，它们不知道热更的界面的类型，知道名字）
        /// </summary>
        /// <param name="viewName">界面名字</param>
        /// <param name="callBack"></param>
        public void OpenWindow(string viewName, Action<UIView> callBack = null)
        {
            if (_activeViews.ContainsKey(viewName))
            {
                Debug.Log($"界面已经打开 {viewName}"); // 根据需求，把界面显示到同层级的最顶层
                return;
            }

            _openViewCommands.Enqueue(new OpenViewCommand()
            {
                ViewName = viewName,
                OnOpen = callBack
            });
        }

        public void CloseWindow<T>()where T : UIView
        { 
            var viewName = typeof(T).Name;
            CloseWindow(viewName);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="viewName">界面名称</param>
        public void CloseWindow(string viewName)
        {
            if (!_activeViews.TryGetValue(viewName, out var view))
            {
                Debug.LogWarning($"找不到界面 {viewName} 来关闭");
                return;
            }

            CloseWindow(view);
        }
        
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="view"></param>
        void CloseWindow(UIView view)
        {
            var viewName = view.ViewInfo.ViewName;
            view.gameObject.SetActive(false);

            _activeViews.Remove(viewName);
            view.OnClose();

            var viewInfo = GetViewInfo(viewName);
            float delayTime = GetDelayTime(viewInfo.ShowFrequency);
            _closeViews.Add(viewName, view);
            _viewLayerManager.CloseView(view);

            if (delayTime == 0)
            {
                DestroyView(view);
            }
            else if (delayTime > 0)
            {
                Debug.LogWarning($"延迟销毁 {viewName}");
                _delayDestroyList.Add(new DelayCloseView()
                {
                    DelayTime = delayTime,
                    viewName = viewName
                });
            }
        }

        /// <summary>
        /// 销毁界面
        /// </summary>
        /// <param name="view"></param>
        void DestroyView(UIView view)
        {
            Debug.Log($"销毁界面 {view.ViewInfo.ViewName}");
            view.OnDestroyView();
            _closeViews.Remove(view.ViewInfo.ViewName);
            GameObject.DestroyImmediate(view.gameObject);
        }

        // public void SetResolution(int width, int height, float matchWithdOrHeight)
        // {
        //     var canvasScaler = _root.GetComponent<CanvasScaler>();
        //     canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //     canvasScaler.referenceResolution = new Vector2(width, height);
        //     canvasScaler.matchWidthOrHeight = matchWithdOrHeight;
        // }

        #region Util

        /// <summary>
        /// 获取界面配置信息
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        UIViewInfo GetViewInfo(string viewName)
        {
            return _viewInfos.Get(viewName);
        }

        /// <summary>
        /// 获取界面层级挂点
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        RectTransform GetLayerParent(E_UILayer layer)
        {
            return _layers.Get(layer);
        }

        /// <summary>
        /// 获取延迟销毁时间
        /// </summary>
        /// <param name="showFrequency"></param>
        /// <returns></returns>
        float GetDelayTime(E_ShowFrequency showFrequency)
        {
            float delayTime = 0;
            switch (showFrequency)
            {
                case E_ShowFrequency.One:
                    delayTime = 0;
                    break;
                case E_ShowFrequency.Normal:
                    delayTime = 3; // 五分钟 300s
                    break;
                case E_ShowFrequency.Offent:
                    delayTime = -1;
                    break;
                default:
                    Debug.LogError("未定义");
                    break;
            }

            return delayTime;
        }

        #endregion
    }
}