using UnityEngine;
using System.Collections;
using UniRx;
namespace ZFramework.Core {
    public class PlayerData 
    {
        public ReactiveProperty<int> Money = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> Lv = new ReactiveProperty<int>(0);
    }
}
