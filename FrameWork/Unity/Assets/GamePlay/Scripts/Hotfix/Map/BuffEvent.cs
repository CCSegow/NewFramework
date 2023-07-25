using GamePlay;
using UnityEngine;

public class BuffEvent : EventBase
{
    private BuffTrigger leftTrigger;//左buff触发事件

    private BuffTrigger rightTrigger;//右buff触发事件

    private bool isTrigger;//该事件是否被触发
    public bool IsTrigger { get { return isTrigger; } set { isTrigger = value; } }

    private void Awake()
    {
        leftTrigger=transform.Find("leftBuff").GetComponent<BuffTrigger>();
        if (leftTrigger != null)
            leftTrigger.BuffEventEX = this;

        rightTrigger=transform.Find("rightBuff").GetComponent<BuffTrigger>();  
        if(rightTrigger!=null)
            rightTrigger.BuffEventEX = this;

    }
    public override void InitData(int eventID)
    {
        var buff = TestData.GetEventTriggerData(eventID);
        leftTrigger.InitData(buff.Ltype, buff.Lvalue);
        rightTrigger.InitData(buff.Rtype, buff.Rvalue);
        eventType = cfg.eventTrigger.type.buff;
    }
    public override void ReSetHpToCount()
    {
    }
}
