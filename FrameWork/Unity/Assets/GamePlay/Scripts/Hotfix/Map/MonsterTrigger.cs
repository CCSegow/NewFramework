using UnityEngine;

public class MonsterTrigger : MonoBehaviour
{
    private int baseHP;//单个怪物血量
    public int BaseHP { get { return baseHP; } set { baseHP = value; } }

    private MonsterEvent monsterEvent;//父类怪物事件类

    private void Awake()
    {
        monsterEvent = transform.parent.GetComponent<MonsterEvent>();
    }
    public void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.GetComponent<CharacterController>();
        if (character != null&&BaseHP>0&&character.BaseHP>0)
        {
            character.MonsterTrigger();
            monsterEvent.MonsterBattleHandle(this);
            baseHP = 0;
            monsterEvent.MonsterDeadHandle(this);
        }
    }
    private bool isBattleMove = false;
    private Vector3 targetPos;
    public void StartHoming(Vector3 pos)
    {
        isBattleMove = true;
        targetPos = pos;
    }
    private void Update()
    {
        if (isBattleMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        }
    }
}
    
   
