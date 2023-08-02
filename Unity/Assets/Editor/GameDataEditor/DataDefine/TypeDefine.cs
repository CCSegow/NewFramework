using UnityEngine;
using Sirenix.OdinInspector;
namespace ZFramework.Core
{
    public class TypeDefine :ScriptableObject
    {
        public string TypeName;
        public string Describe;
        [TableList]
        public TypeItem[] SubTypes;
    }
    [System.Serializable]
    public struct TypeItem {
        public string Name;
        public string Describe;
    }
}
