using GamePlay;
using System.Collections.Generic;
using UnityEngine;
using ZFramework.Core;

public class EventTriggerMgr : MonoBehaviour
{
    private int baseEventInterval;
    private int curEventInterval;
    private List<EventBase> events;
    private List<int> eventIds;

    private EventBase eventBase;//事件物体资源

    private void Awake()
    {
        GamePlayData.Instance.EventTriggerMgrEx = this;
        events=new List<EventBase>();
        eventIds = new List<int>();
        InitData();
    }
    private void InitData()
    {
        var mapData = TestData.GetMapData(GamePlayData.Instance.CurMapID);
        foreach(var id in mapData.EventId)
        {
            eventIds.Add(id);
        }
        baseEventInterval = GamePlayData.Instance.EventInterval;
        curEventInterval = baseEventInterval;
    }
    private void OnDestroy()
    {
        GamePlayData.Instance.EventTriggerMgrEx = null;
    }
    public void ReSetHpToCount()
    {
        foreach (var e in events)
        {
            e.ReSetHpToCount();
        }
    }
    public void CreateEvents(float zPos)
    {
        do
        {
            CreateEvent();
            curEventInterval += baseEventInterval;
        } while (curEventInterval < zPos);
    }
    private void CreateEvent()
    {
        if (eventIds.Count <= 0) return;
        int id = eventIds[0];
        var eventEx=TestData.GetEventTriggerData(id);
        LoadAssets(eventEx.EventPath);
        eventIds.RemoveAt(0);
        EventBase _base = Instantiate(eventBase, transform);
        if (_base != null)
        {
            Vector3 buffLocalPos = new Vector3(0, 0, curEventInterval);
            _base.transform.localPosition = buffLocalPos;
            _base.InitData(id);
            events.Add(_base);
        }
    }
    /// <summary>
    /// 加载所需物体资源
    /// </summary>
    private void LoadAssets(string path)
    {
        var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
        var servant = assetComponent.GetServant();

        eventBase = servant.GetAsset<EventBase>(path);

        //servant.DisposeAll();
    }
    private bool isOver = false;
    public bool CheckEventOver()
    {
        if (!isOver && eventIds.Count == 0)
        {
            isOver = true;
            return isOver;
        }
        return false;
    }
    public void ReChallenge()
    {
        eventIds.Clear();
        for (int i = 0; i < events.Count; i++)
        {
            if(null!= events[i])
                Destroy(events[i].gameObject);
        }
        isOver = false;
        events.Clear();
        InitData();
    }
}
