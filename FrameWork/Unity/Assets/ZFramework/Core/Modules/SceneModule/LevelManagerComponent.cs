using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ZFramework.Core {
    public class LevelManagerComponent : ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(LevelManagerComponent);
        LevelSetting _levelSetting;
        int _curLevelIndex;


        protected override void OnInit()
        {
            //加载场景配置
        }

        public void LoadCurrent() { 
            //SceneMgr.Ins.LoadScene(LevelName(_curLevelIndex), UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void LoadNext() {
            _curLevelIndex++;
            if (_curLevelIndex >= LevelCount()) {
                _curLevelIndex = LevelCount() - 1;
            }
            LoadCurrent();
        }
        #region Util
        string LevelName(int index) {
            return _levelSetting.LevelInfos[index].LevelName;
        }
        int LevelCount() {
            return _levelSetting.LevelInfos.Length;
        }
        #endregion
    }

}
