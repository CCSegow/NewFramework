using System.Collections.Generic;
using UnityEngine;
using ZFramework.Core;
using GamePlay;

public class CharacterMgr : MonoBehaviour
{
    private int totalHP;//人物总血量
    public int TotalHP => totalHP;

    private List<CharacterController> characterControllers;//人物集合
    public List<CharacterController> CharacterControllers => characterControllers;

    private CharacterController CharacterController;//人物预制体

    private Vector3 horizontalMomentum;//X轴动量，由玩家操作决定
    private Vector3 verticalMomentum;//Z轴动量，固定向前
    private Vector3 moveMomentum;//和动量

    private int characterIndex;//角色编号

    private Transform characterView;//相机

    private bool allowMove;//允许移动

    private float ringRadius = 0;
    private bool startBattle = false;

    private void Awake()
    {
        characterControllers = new List<CharacterController>();
        verticalMomentum = Vector3.forward;
        characterIndex = 0;
        characterView = transform.Find("CharacterView");

        GamePlayData.Instance.CharacterMgrEx = this;
    }
    private void OnDestroy()
    {
        GamePlayData.Instance.CharacterMgrEx = null;
    }
    private void InitData()
    {
        var character = TestData.GetCharacterData(GamePlayData.Instance.CurCharactersID);
        string characterPath = character.Path;
        LoadAssets(characterPath);
        totalHP = character.TotalHp;
       
    }
    /// <summary>
    /// 加载角色模型资源
    /// </summary>
    private void LoadAssets(string path)
    {
        var assetComponent = GameManager.Ins.GetGameComponent<AssetManagerComponent>();
        var servant = assetComponent.GetServant();
        CharacterController = servant.GetAsset<CharacterController>(path);
        //servant.DisposeAll();
    }
    /// <summary>
    /// 生成一个角色
    /// </summary>
    /// <param name="target">生成角色的目标点</param>
    private void CreateCharacter(Vector3 target)
    {
        var character = Instantiate(CharacterController, new Vector3(target.x,0,target.z), characterView.rotation, transform);
        character.name = $"character {characterIndex}";
        character.CharacterMgrEX = this;
        characterControllers.Add(character);
        characterIndex++;
    }
    /// <summary>
    /// 战斗开始，初始化队伍人物模型
    /// </summary>
    public void InitCharacter(Vector3 spwanPoint)
    {
        InitData();
        var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
        uiMgr.OpenWindow<GameDataView>(view =>
        {
            view?.RefreshTotalHP(totalHP.ToString());
        });
        GamePlayData.Instance.ReSetCurHpToCount(totalHP);
        characterView.position = spwanPoint;
        //TODO 获取血量模型个数比例，来决定xx血生成xx人
        int count = totalHP / GamePlayData.Instance.CurHpToCount;
        _radius = CharacterController.GetComponent<CapsuleCollider>().radius * 3;
        GetCharacterPos(count);
        for (int i = 0; i < count; i++)
        {
            CreateCharacter(spwanPoint + characterPos[i]);
        }
        allowMove = true;
        ShowHPText();
        ResetCharacterBaseHp();
    }
    /// <summary>
    /// 角色整体移动
    /// </summary>
    private void Update()
    {
        if (allowMove && !startBattle)
        {
            float x = characterControllers[0].transform.localPosition.x;
            horizontalMomentum = Vector3.zero;
            if (Input.GetKey(KeyCode.A)&& x >= GamePlayData.Instance.LeftBoundary + ringRadius)
                horizontalMomentum = Vector3.left;
            if (Input.GetKey(KeyCode.D) && x <= GamePlayData.Instance.RightBoundayr - ringRadius)
                horizontalMomentum = Vector3.right;
           
           
            moveMomentum = horizontalMomentum + verticalMomentum;

            for (int i = 0; i < characterControllers.Count; i++)
            {
                characterControllers[i].Move(moveMomentum+new Vector3(characterView.position.x,0,characterView.position.z)
                    + characterPos[i]);
            }
        }
    }
    /// <summary>
    /// 相机view的移动，限制距离
    /// </summary>
    private void LateUpdate()
    {
        if (allowMove && !startBattle)
        {
            characterView.position = Vector3.Lerp(characterView.position, characterControllers[0].transform.position
               , Time.deltaTime * GamePlayData.Instance.CurMoveSpeed);
        }
    }
    /// <summary>
    /// 血量变化后处理
    /// </summary>
    private void ReSetCharacterModels()
    {
        GamePlayData.Instance.ReSetCurHpToCount(totalHP);
        //TODO 血量改变， 
        int count = (totalHP / GamePlayData.Instance.CurHpToCount);
        int preCount = characterControllers.Count;
        GetCharacterPos(count);
        if (count < preCount)
        {
            for (int i = 0; i < preCount - count; i++)
            {
                CharacterDead(characterControllers[characterControllers.Count - 1]);
            }
        }
        else if (count > preCount)
        {
            for (int i = characterControllers.Count; i < count; i++)
            {
                CreateCharacter(characterView.position + characterPos[i]);
            }
        }
    }
    private void ResetCharacterBaseHp()
    {
        int baseHP = totalHP / characterControllers.Count;
        for (int i = 0; i < characterControllers.Count; i++)
        {
            characterControllers[i].BaseHP = baseHP;
        }
        GameLoseHandle();
    }
    private List<Vector3> characterPos = new List<Vector3>();
    private float _radius;
    /// <summary>
    /// 得到所有角色围绕点的坐标
    /// </summary>
    /// <param name="count">角色数量</param>
    private void GetCharacterPos(int count)
    {
        if (count <= 1)
            return;

        characterPos.Clear();

        int rings = 1;
        int ringIndex = 0;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Vector3.zero;
            if (ringIndex < Mathf.Pow(rings, 2))
            {
                ringIndex++;
            }
            else
            {
                rings++;
                ringIndex = 0;
            }
            pos = EvaluatePoints(rings, ringIndex, (int)Mathf.Pow(rings, 2));
            characterPos.Add(pos);
        }
        ringRadius = rings * _radius;
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
    private Vector3 EvaluatePoints(int rings, int ringIndex, int amount)
    {
        var angle = ringIndex * Mathf.PI * (2 * _rotations) / ++amount + (rings % 2 != 0 ? _nthOffset : 0);
        var radius = _radius + ringIndex * _radiusGrowthMultiplier;
        var x = Mathf.Cos(angle) * radius * (rings - 1);
        var z = Mathf.Sin(angle) * radius * (rings - 1);
        var pos = new Vector3(x, 0, z);
        pos *= Spread;
        return pos;
    }
   

   
    /// <summary>
    /// 触发buff事件
    /// </summary>
    /// <param name="type">buff类型</param>
    /// <param name="value">buff数值</param>
    public void BuffTrigger(cfg.eventTrigger.type type, int value)
    {
        switch (type)
        {
            case cfg.eventTrigger.type.add:
                totalHP += value; break;
            case cfg.eventTrigger.type.subtract:
                totalHP -= value; break;
            case cfg.eventTrigger.type.multiply:
                totalHP *= value; break;
            case cfg.eventTrigger.type.divide:
                totalHP /= value; break;
            default:
                break;
        }
        ShowHPText();
        ReSetCharacterModels();
        ResetCharacterBaseHp();
    }
    /// <summary>
    /// 触发怪物事件
    /// </summary>
    /// <param name="dmage">伤害值</param>
    public void MonsterTrigger(CharacterController character)
    {
        totalHP -= character.BaseHP;
        allowMove = false;
        CharacterDead(character);
        ShowHPText();
        GameLoseHandle();
    }
    /// <summary>
    /// 角色死亡
    /// </summary>
    /// <param name="character">角色</param>
     private void CharacterDead(CharacterController character)
    {
        bool isFind = false;
        Vector3 curPos = Vector3.zero;
        Vector3 prePos = character.transform.position;
        for (int i=0;i< characterControllers.Count;i++)
        {
            if (isFind)
            {
                curPos = characterControllers[i].transform.position;
                characterControllers[i].StartHoming(prePos);
                prePos = curPos;
            }
            if (characterControllers[i]==character)
            {
                isFind = true;
            }
        }
        
        characterControllers.Remove(character);
        Destroy(character.gameObject);
    }
    /// <summary>
    /// 开始战斗，所有角色停止移动
    /// </summary>
    public void StartBattle()
    {
        if (startBattle) return;
        startBattle = true;
        foreach (CharacterController characterController in characterControllers)
        {
            characterController.StopMove();
        }
    }
    /// <summary>
    /// 战斗结束
    /// </summary>
    public void MonsterBattleOver()
    {
        startBattle = false;
        allowMove = true;
        foreach (CharacterController characterController in characterControllers)
        {
            characterController.EndHoming();
        }
        ReSetCharacterModels();
        ResetCharacterBaseHp();
    }
    /// <summary>
    /// 障碍物触发事件
    /// </summary>
    /// <param name="dmage">伤害值</param>
    public void ObstacleTrigger(CharacterController character)
    {
        totalHP -= character.BaseHP;
        characterControllers.Remove(character);
        Destroy(character.gameObject);
        ShowHPText();
        ReSetCharacterModels();
        ResetCharacterBaseHp();
        GameLoseHandle();
    }
    /// <summary>
    /// 展示总血量文本
    /// </summary>
    private void ShowHPText()
    {
        var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
        uiMgr.GetView<GameDataView>((view) =>
        {
            view?.RefreshTotalHP(totalHP.ToString());
        });
    }
    
    /// <summary>
    /// 抵达终点，游戏胜利
    /// </summary>
    public void GameWinHandle()
    {
        //TODO 播放结算界面
        allowMove = false;
        foreach (CharacterController characterController in characterControllers)
        {
            characterController.StopMove();
        }
        var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
        uiMgr.CloseWindow<GameDataView>();
        uiMgr.OpenWindow<GameSettleView>((view) =>
        {
            view.Bind.Get_tip_text.text = $"WINNER";
        });
    }
    /// <summary>
    /// 血量归0，游戏失败
    /// </summary>
    private void GameLoseHandle()
    {
        if (totalHP <= 0)
        {
            allowMove = false;
            //TODO 播放结算界面
            var uiMgr = GameManager.Ins.GetGameComponent<UIManagerComponent>();
            uiMgr.CloseWindow<GameDataView>();
            uiMgr.OpenWindow<GameSettleView>((view) =>
            {
                view.Bind.Get_tip_text.text = $"LOSER";
            });
        }
    }
    public void ReChallenge()
    {
        for(int i=0;i<characterControllers.Count;i++)
        {
            Destroy(characterControllers[i].gameObject);
        }
        characterControllers.Clear();

    }


}
