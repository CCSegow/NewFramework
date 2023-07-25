using GamePlay;
public class ObstacleEvent : EventBase
{
    private ObstacleTrigger obstacleTrigger;
    private int eventValue;
    private void Awake()
    {
        obstacleTrigger=transform.Find("ObstacleTrigger").GetComponent<ObstacleTrigger>();
        obstacleTrigger.ObstacleEventMethod = this;
    }
    /// <summary>
    /// 障碍物事件的触发反馈
    /// </summary>
    /// <param name="character">人物行为控制类</param>
    public void StartTrigger(CharacterController character)
    {
        character.ObstacleTrigger();
    }
    public override void InitData(int eventID)
    {
        var obstacle = TestData.GetEventTriggerData(eventID);
        eventType = obstacle.Ltype;
        eventValue = obstacle.Lvalue;
    }
    public override void ReSetHpToCount()
    {
    }
}
