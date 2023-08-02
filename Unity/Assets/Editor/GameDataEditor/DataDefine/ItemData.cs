using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
public class ItemData : ScriptableObject
{
    [HorizontalGroup("Base",LabelWidth = 80)]
    
    [VerticalGroup("Base/Left")]
    [BoxGroup("Base/Left/基本设置")]
    [HorizontalGroup("Base/Left/基本设置/r1")]
    public string Name;
    [HorizontalGroup("Base/Left/基本设置/r1"),PreviewField(64)]
    public Sprite Icon;

    [HorizontalGroup("Base/Left/基本设置/r2"),LabelText("物品类型")]    
    public ItemRareType RareType;

    [HorizontalGroup("Base/Left/基本设置/r2"), LabelText("价格")]
    public int Price;

    [HorizontalGroup("Base/Left/基本设置/r2"), LabelText("可消耗"),ValueDropdown("Togglable")]
    public bool Consumable;

    [HorizontalGroup("Base/Left/基本设置/r3"), LabelText("作用范围")]
    public ItemRareType EffectRange;
    [HorizontalGroup("Base/Left/基本设置/r3"), LabelText("使用场合")]
    public UseScene UseScene;


    [VerticalGroup("Base/Right")]
    [BoxGroup("Base/Right/效果")]
    [HorizontalGroup("Base/Right/效果/r3"), LabelText("作用值")]
    public float EffectValue;

    
    [BoxGroup("Base/Right/备注")]
    [TextArea(5, 10), HideLabel]
    public string Note;

    private IEnumerable Togglable = new ValueDropdownList<bool>() {
        {"Yes",true },
        {"No",false }
    };
}

public enum ItemRareType { 
    普通,
    稀有,
    传说,
    任务物品
}
public enum TargetRange {
    [LabelText("无")]
    None,
    [LabelText("敌方单体")]
    SingleEnemy,
    [LabelText("敌方全体")]
    AllEnemy,
    [LabelText("我方单体")]
    SingleTeammate,
    [LabelText("我方全体")]
    AllTeammate
}
public enum UseScene
{
    [LabelText("随时可用")]
    AnyTime,
    [LabelText("仅战斗中")]
    InBattle,
    [LabelText("仅城镇中")]
    InCity,
    [LabelText("不可用")]
    CantUse    
}


public struct ItemProperty { 
    
}
