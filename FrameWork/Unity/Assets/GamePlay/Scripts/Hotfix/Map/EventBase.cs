using UnityEngine;
public abstract class EventBase : MonoBehaviour
{
    protected cfg.eventTrigger.type eventType;
    public abstract void InitData(int eventID);

    public abstract void ReSetHpToCount();
}
