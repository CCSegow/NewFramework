using System;
using UnityEngine;
using System.Collections;
namespace ZFramework.Core 
{
    public class SaveLoadComponent :ZeroFrameWorkComponent
    {
        public override Type GetComponentType => typeof(SaveLoadComponent);
        public bool IsNewPlayer { get; private set; }
        public PlayerData PlayerData;


        protected override void OnInit()
        {
            
        }

        void Load() {
            PlayerData = new PlayerData();
        }

        void Save() { 
        
        }

        
    }

}
