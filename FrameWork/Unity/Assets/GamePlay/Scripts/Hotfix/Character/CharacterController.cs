using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    private int baseHP;//单个角色血量
    public int BaseHP { get { return baseHP; }set { baseHP = value; } }

    private CharacterMgr characterMgr;//人物管理类
    public CharacterMgr CharacterMgrEX { get { return characterMgr; } set { characterMgr = value; } }

    private Rigidbody rb;//角色刚体

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// 角色移动，管理类统一调用
    /// </summary>
    /// <param name="momentum">动量</param>
    public void Move(Vector3 TargetPos)
    {
        Vector3 momentum = (TargetPos - transform.position).normalized;
        rb.velocity = momentum * GamePlayData.Instance.CurMoveSpeed;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 碰到buff，触发buff事件
    /// </summary>
    public void BuffTrigger(cfg.eventTrigger.type type, int value)
    {
        //TODO 简单加减乘除buff，通知管理器
        characterMgr.BuffTrigger(type, value) ;
    }

    /// <summary>
    /// 遭遇怪物，触发怪物事件
    /// </summary>
    public void MonsterTrigger()
    {
        characterMgr.MonsterTrigger(this);
        baseHP = 0;
        //TODO 与怪物发生战斗，扣血，反馈给管理器
    }
    /// <summary>
    /// 碰到障碍物，触发障碍物事件
    /// </summary>
    public void ObstacleTrigger()
    {
        //TODO 被障碍物波及，可能发生死亡，通知管理器
        characterMgr.ObstacleTrigger(this);
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
        if(isBattleMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * GamePlayData.Instance.CurMoveSpeed);
            if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
                EndHoming();
        }
    }
    public void EndHoming()
    {
        isBattleMove = false;
    }
}
