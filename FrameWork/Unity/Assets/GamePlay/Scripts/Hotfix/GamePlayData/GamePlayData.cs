using GamePlay;
using System.Collections.Generic;

public class GamePlayData
{
    private static GamePlayData instance;
    public static GamePlayData Instance
    {
        get
        {
            if (instance == null)
                instance = new GamePlayData();
            return instance;
        }
    }
    public GamePlayData()
    {
        InitCommonData();
    }
    private int curMapID = 101;
    public int CurMapID { get { return curMapID; } set { curMapID = value; } }

    private int curCharactersID = 3;
    public int CurCharactersID { get { return curCharactersID; } set { curCharactersID = value; } }

    private float curMoveSpeed = 10f;
    public float CurMoveSpeed { get { return curMoveSpeed; }set { curMoveSpeed = value; } }

    private CharacterMgr characterMgr;
    public CharacterMgr CharacterMgrEx { get { return characterMgr; } set {  characterMgr = value; } }

    private MapBlockManager mapBlockMgr;
    public MapBlockManager MapBlockMgrEx { get { return mapBlockMgr; } set {  mapBlockMgr = value; } }

    private EventTriggerMgr eventTriggerMgr;
    public EventTriggerMgr EventTriggerMgrEx { get { return eventTriggerMgr; } set { eventTriggerMgr = value; } }

    private Dictionary<int, int> hp_to_count;
    public Dictionary<int, int> HPToCount => hp_to_count;

    private int curHpToCount = 1;
    public int CurHpToCount => curHpToCount;

    private int commonIndex;

    private int eventInterval;
    public int EventInterval => eventInterval;

    private float leftBoundary = -7.5f;
    public float LeftBoundary => leftBoundary;
    private float rightBoundary = 7.5f;
    public float RightBoundayr => rightBoundary;

    private void InitCommonData()
    {
        commonIndex = 0;
        cfg.demo.Common common = null;
        hp_to_count=new Dictionary<int, int>();
        do
        {
            common = TestData.GetCommonData(commonIndex);
            if(common!=null)
            {
                if (commonIndex == 0)
                    eventInterval = common.EventInterval;
                hp_to_count.Add(common.NumMaximum, common.HpToCount);
                commonIndex++;
            }
           
        } while (common != null);
    }
    public void ReSetCurHpToCount(int totalHP)
    {

        foreach (var hp in hp_to_count)
        {
            if (totalHP >= hp.Key)
                curHpToCount = hp.Value;
            else
                break;
        }

        eventTriggerMgr.ReSetHpToCount();
    }
}
