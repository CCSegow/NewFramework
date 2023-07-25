using UnityEngine;
using System.Collections;
namespace ZFramework.Core {
    public interface IFactory 
    {
        object CreateNew();
    }
}
