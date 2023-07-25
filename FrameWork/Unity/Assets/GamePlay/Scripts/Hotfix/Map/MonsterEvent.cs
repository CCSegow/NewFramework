using GamePlay;
using System.Collections.Generic;
using UnityEngine;
using ZFramework.Core;

public class MonsterEvent : EventBase
{
    private MonsterTrigger monster_trigger;//怪物资源
    private List<MonsterTrigger> monsters;//本事件下的怪物集合

    private int totalHP;//总血量
    private TextMesh totalHPTextMesh;

    private List<CharacterController> enemys;
    public List<CharacterController> Enemys { get { return enemys; } set { enemys = value; } }
    private void Awake()
    {
        monsters = new List<MonsterTrigger>();
        totalHPTextMesh=transform.Find("_totalHP_text_mesh").GetComponent<TextMesh>();  
        enemys = new List<CharacterController>();
    }
    /// <summary>
    /// 加载怪物模型资源
    /// </summary>
    private void LoadAssets(string path)
    {
        var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
        var servant = assetComponent.GetServant();
        monster_trigger = servant.GetAsset<MonsterTrigger>(path);
        _radius = monster_trigger.gameObject.GetComponent<CapsuleCollider>().radius * 3;
        //servant.DisposeAll();
    }
    private int monsterIndex = 0;
    /// <summary>
    /// 根据本事件怪物数量生成怪物模型，均分血量
    /// </summary>
    private void AddMonsterHandle()
    {
        int count = totalHP / GamePlayData.Instance.CurHpToCount;
        GetMonsterPos(count);
        for (int i=0;i< count; i++)
        {
            CreateMonster(i);
        }
    }
    private void CreateMonster(int index)
    {
        MonsterTrigger monster = Instantiate(monster_trigger, transform.position + MonsterPos[index],
               transform.rotation * Quaternion.Euler(new Vector3(0, 180, 0)), transform);
        if (monster != null)
        {
            monster.BaseHP = GamePlayData.Instance.CurHpToCount;
            monster.name = $"monster {monsterIndex}";
            monsterIndex++;
            monsters.Add(monster);
        }
    }
    /// <summary>
    /// 进入战斗后，怪物触发通知事件战斗情况，扣除怪物总血量
    /// </summary>
    /// <param name="dmage">伤害值</param>
    public void MonsterBattleHandle(MonsterTrigger monster)
    {
        totalHP -= monster.BaseHP;
        ShowHPDesc();
        if (totalHP <= 0)
        {
            GamePlayData.Instance.CharacterMgrEx.MonsterBattleOver();
            Destroy(this.gameObject);
        }
        //TODO 回收进对象池
    }
    /// <summary>
    /// 怪物死亡
    /// </summary>
    /// <param name="monster"></param>
    public void MonsterDeadHandle(MonsterTrigger monster)
    {
        monsters.Remove(monster);
        Destroy(monster.gameObject);
        //TODO 回收怪物进入对象池
    }
    /// <summary>
    /// 显示怪物事件总血量
    /// </summary>
    private void ShowHPDesc()
    {
        totalHPTextMesh.text = string.Format("{0}", totalHP);
    }
    public override void InitData(int eventID)
    {
        var monster = TestData.GetEventTriggerData(eventID);
        string triggerPath = "Assets/Bundles/Prefabs/Map/Monster_Dog.prefab";
        totalHP = monster.Lvalue;
        eventType = monster.Ltype;
        LoadAssets(triggerPath);
        AddMonsterHandle();
        ShowHPDesc();
    }
    private List<Vector3> MonsterPos = new List<Vector3>();
    private float _radius;
    /// <summary>
    /// 得到所有角色围绕点的坐标
    /// </summary>
    /// <param name="count">角色数量</param>
    private void GetMonsterPos(int count)
    {
        if (count <= 1)
            return;

        MonsterPos.Clear();

        int rings = 2;
        int ringIndex = 0;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Vector3.zero;
            if (i == 0)
            { }
            else
            {
                if (ringIndex < Mathf.Pow(2, rings))
                {
                    ringIndex++;
                }
                else
                {
                    rings++;
                    ringIndex = 0;
                }
                pos = EvaluatePoints(rings, ringIndex, (int)Mathf.Pow(2, rings));
            }
            MonsterPos.Add(pos);
        }
    }
    private float _radiusGrowthMultiplier = 0;
    private float _rotations = 1;
    private float _nthOffset = 0;
    private float Spread = 1f;
    /// <summary>
    /// 获取某角色围绕环的坐标
    /// </summary>
    /// <param name="rings">第几环</param>
    /// <param name="ringIndex">环中的第几个</param>
    /// <param name="amount">该环所有角色的数量</param>
    /// <returns></returns>
    public Vector3 EvaluatePoints(int rings, int ringIndex, int amount)
    {
        var angle = ringIndex * Mathf.PI * (2 * _rotations) / amount + (rings % 2 != 0 ? _nthOffset : 0);
        var radius = _radius + ringIndex * _radiusGrowthMultiplier;
        var x = Mathf.Cos(angle) * radius * (rings - 1);
        var z = Mathf.Sin(angle) * radius * (rings - 1);
        var pos = new Vector3(x, 0, z);
        pos *= Spread;
        return pos;
    }
    public override void ReSetHpToCount()
    {
        int count = totalHP / GamePlayData.Instance.CurHpToCount;
        int preCount = monsters.Count;
        GetMonsterPos(count);
        if (count < preCount)
        {
            for (int i = 0; i < preCount - count; i++)
            {
                MonsterDeadHandle(monsters[monsters.Count - 1]);
            }
        }
        else if (count > preCount)
        {
            for (int i = preCount; i < count; i++)
            {
                CreateMonster(i);
            }
        }
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].BaseHP = GamePlayData.Instance.CurHpToCount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController character=other.GetComponent<CharacterController>();
        if(character != null&&!isFighting)
        {
            isFighting = true;
            GamePlayData.Instance.CharacterMgrEx.StartBattle();
        }
    }
    private bool isFighting = false;
    private void Update()
    {
        if(isFighting&&GamePlayData.Instance.CharacterMgrEx.CharacterControllers.Count>0)
        {
            transform.position = Vector3.Lerp(transform.position,
                GamePlayData.Instance.CharacterMgrEx.CharacterControllers[0].transform.position, Time.deltaTime);
            for(int i=0;i<monsters.Count;i++)
            {
                monsters[i].StartHoming(MonsterPos[i]+transform.position);
            }
        }
    }
}
