using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZFramework.GamePlay {

    public class TimeTickItem {
        public float LifeTime;
        public Action OnEnd;
        public Action OnPerTick;

        public void Tick() {
            LifeTime -= 1;
            OnPerTick?.Invoke();
            if (LifeTime == 0) {
                OnEnd?.Invoke();
            }
        }
    }
    public class TimeTickSystem
    {
        List<TimeTickItem> timeTicks;

        public Action OnTick;

        private float _tickTimer;        


        public TimeTickSystem() 
        {
            timeTicks = new List<TimeTickItem>();
        }

        public TimeTickItem AddTickItem(float lifeTime,Action onEnd,Action onPerTick) {
            var tickItem = new TimeTickItem { 
                LifeTime = lifeTime,
                OnEnd = onEnd,
                OnPerTick = onPerTick
            };
            timeTicks.Add(tickItem);
            return tickItem;
        }
        public void RemoveTickItem(TimeTickItem tickItem) { 
            timeTicks.Remove(tickItem);
        }

        public void Update() {
            _tickTimer += Time.deltaTime;
            if (_tickTimer >= 1) {
                _tickTimer = 0;
                for (int i = timeTicks.Count - 1; i>= 0 ; i--){
                    timeTicks[i].Tick();

                    if (timeTicks[i].LifeTime == 0) {
                        timeTicks.RemoveAt(i);
                    }
                }

                OnTick?.Invoke();
            }
        }
    }

}

