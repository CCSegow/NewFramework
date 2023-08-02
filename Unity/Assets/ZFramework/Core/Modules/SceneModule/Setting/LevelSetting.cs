using UnityEngine;
using System.Collections;
namespace ZFramework.Core {
    [System.Serializable]
    public class LevelInfo {
        public string LevelName;
    }
    
    public class LevelSetting : ScriptableObject
    {
        public LevelInfo[] LevelInfos;
    }

}
