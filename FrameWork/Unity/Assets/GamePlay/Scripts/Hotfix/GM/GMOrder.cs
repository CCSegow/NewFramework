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
            AddValueOpt("��ǰ�ؿ�ID", () => GamePlayData.Instance.CurMapID,
                v => { GamePlayData.Instance.CurMapID = v;
                    RefreshView<StartGameView>();
                }, "1.�޸������ؿ���");

            AddValueOpt("��ǰ��ɫID", () => GamePlayData.Instance.CurCharactersID,
               v => { GamePlayData.Instance.CurCharactersID = v;
                   RefreshView<StartGameView>();
               }, "1.�޸�������ɫ��");
            AddValueOpt("��ɫ�ƶ��ٶ�", () => GamePlayData.Instance.CurMoveSpeed,
              v => {
                  GamePlayData.Instance.CurMoveSpeed = v;
              }, "1.�޸�������ɫ��");
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
