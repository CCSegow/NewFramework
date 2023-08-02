using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ZFramework.Core;

namespace ZFramework {

    // 侦听消息辅助容器
    // 通过此容器统一对当前持有者的所有注册消息进行管理
    // 统一注册和释放，来避免忘记注销的失误

    /// <summary>
    /// 不支持重复注册同一个事件
    /// 但是可以把关心同一事件的方法放到一起然后再进行注册
    /// 即，单注册，同时注销
    /// （以类为力度对事件进行侦听）
    /// </summary>
    public class ClassMessageRegister:IDisposable
    {
        Dictionary<string, Action<object>> _allEvents;

        public ClassMessageRegister() {
            _allEvents = new Dictionary<string, Action<object>>();
        }
        
        /// <summary>
        /// 一个类只能对某个事件进行一次注册
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="callBack"></param>
        public void Register(string msgName,Action<object> callBack) {
            MessageDispatcher.Register(msgName,callBack);
            _allEvents.Add(msgName,callBack);
        }
        public void UnRegister(string msgName) {
            if (_allEvents.TryGetValue(msgName, out Action<object> callBack)) {
                Debug.Log($"删除 {msgName} 事件的所有侦听者");
                _allEvents.Remove(msgName);
                MessageDispatcher.UnRegister(msgName,callBack);
            }
        }
        public void Dispose()
        {
            foreach (var item in _allEvents)
            {
                MessageDispatcher.UnRegister(item.Key, item.Value);
            }
            _allEvents.Clear();
            _allEvents = null;
        }
    }

    /// <summary>
    /// 可以重复注册，可以按需注销，取消侦听摸个事件时，不会影响到当前类其他侦听该事件的方法
    /// (以方法为粒度对事件进行侦听)
    /// </summary>
    public class MethodMessageRegisterForeach : IDisposable
    {
        public class MsgRecord: IPoolableObject
        {
            public string Name;
            public Action<object> CallBack;
        }

        [Factory(true)]
        public class MsgRecordFactory : BaseFactory<MsgRecord>
        {
            //public override MsgRecord CreateNew()
            //{
            //    return new MsgRecord();
            //}
            public override object CreateNew()
            {
                return new MsgRecord();
            }
        }

        List<MsgRecord>  _allEvents;

        public MethodMessageRegisterForeach()
        {
            _allEvents = new List<MsgRecord>();
        }
        
        /// <summary>
        /// 可以对某个事件进行多次注册
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="callBack"></param>
        public void Register(string msgName, Action<object> callBack)
        {
            MessageDispatcher.Register(msgName, callBack);
            var recordItem = PoolUtil<MsgRecord>.Get();
            recordItem.Name = msgName;
            recordItem.CallBack = callBack;
            _allEvents.Add(recordItem);
        }
        
        /// <summary>
        /// 取消事件的注册
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="callBack"></param>
        public void UnRegister(string msgName, Action<object> callBack)
        {
            for (int i = _allEvents.Count - 1; i >= 0; i--)
            {
                var item = _allEvents[i];
                if (item.CallBack == callBack)
                {
                    MessageDispatcher.UnRegister(item.Name, item.CallBack);
                    _allEvents.RemoveAt(i);
                    PoolUtil<MsgRecord>.Cache(item);
                    return;
                }
            }
        }
        public void UnRegisterAll(string msgName)
        {
            for (int i = _allEvents.Count - 1; i >= 0; i--)
            {
                var item = _allEvents[i];
                if (item.Name == msgName)
                {
                    MessageDispatcher.UnRegister(item.Name, item.CallBack);
                    _allEvents.RemoveAt(i);
                    PoolUtil<MsgRecord>.Cache(item);
                }
            }
        }
        public void Dispose()
        {
            foreach (var item in _allEvents)
            {
                MessageDispatcher.UnRegister(item.Name, item.CallBack);                
            }
            PoolUtil<MsgRecord>.ClearPool();
            _allEvents.Clear();
            _allEvents = null;
        }
    }
}
