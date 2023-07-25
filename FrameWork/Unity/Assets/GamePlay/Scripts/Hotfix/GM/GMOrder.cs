using System;
using UnityEngine;
using SRDebugger;
using ZFramework.Core;
using GamePlay;

namespace GamePlay
{
    public class GMOrder :MonoBehaviour
    {
        private static int _optPriority = 0;
        private void Awake()
        {
            AddOrder();
        }
        private static void AddOrder()
        {
            AddValueOpt("当前关卡ID", () => GamePlayData.Instance.CurMapID,
                v => { GamePlayData.Instance.CurMapID = v;
                    RefreshView<StartGameView>();
                }, "1.修改器（关卡）");

            AddValueOpt("当前角色ID", () => GamePlayData.Instance.CurCharactersID,
               v => { GamePlayData.Instance.CurCharactersID = v;
                   RefreshView<StartGameView>();
               }, "1.修改器（角色）");
            AddValueOpt("角色移动速度", () => GamePlayData.Instance.CurMoveSpeed,
              v => {
                  GamePlayData.Instance.CurMoveSpeed = v;
              }, "1.修改器（角色）");
        }
        private static void RefreshView<T>() where T : UIView
        {
            var viewMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            var view = viewMgr.GetView<T>() as T;
            if (view != null)
                view.RefreshView();
        }
        static void AddValueOpt<T>(string name, Func<T> getter, Action<T> setter = null, string category = "Default",
            int sortPriority = 0)
        {
            if (sortPriority == 0)
            {
                sortPriority = ++_optPriority;
            }

            var value = OptionDefinition.Create(name, getter, setter, category, sortPriority);
            SRDebug.Instance.AddOption(value);
        }

        static void AddFunc(string btnName, Action onClick, string category = "Default", int sortPriority = 0)
        {
            if (sortPriority == 0)
            {
                sortPriority = ++_optPriority;
            }

            var func = OptionDefinition.FromMethod(btnName, onClick, category, sortPriority);
            SRDebug.Instance.AddOption(func);
        }

    }
}
