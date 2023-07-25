using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace ZFramework.Core
{
    public class URLConfigs : ScriptableObject
    {
        [FolderPath(AbsolutePath = true)]
        public string DevelopingURL;
        [FolderPath]
        public string TestURL;
        [FolderPath]
        public string ReleaseURL;
    }
}