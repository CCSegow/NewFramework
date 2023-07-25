using UnityEngine;
using UnityEngine.UI;
public class BuffTrigger : MonoBehaviour
{
    private cfg.eventTrigger.type buffType;//buff类型（加，减，乘，除）
    private int buffValue;//buff数值

    public Text desc;//文字描述

    private BuffEvent buffEvent;//buff事件
    public BuffEvent BuffEventEX { get { return buffEvent; } set { buffEvent = value; } }
    
    /// <summary>
    /// 角色触碰到buff，反馈
    /// </summary>
    /// <param name="other">角色</param>
    public void OnTriggerEnter(Collider other)
    {
        //一个buff事件只触发一次
        if(!buffEvent.IsTrigger)
        {
            CharacterController controller=other.GetComponent<CharacterController>();  
            if(controller != null )
            {
                controller.BuffTrigger(buffType, buffValue);
                buffEvent.IsTrigger = true;
            }
        }
    }
    public void InitData(cfg.eventTrigger.type type, int value)
    {
        buffType = type;
        buffValue = value;
        InitDesc();
    }
    /// <summary>
    /// 根据类型和数值初始化文本
    /// </summary>
    private void InitDesc()
    {
        switch (buffType)
        {
            case cfg.eventTrigger.type.add:
                desc.text = string.Format("+{0}", buffValue); break;
            case cfg.eventTrigger.type.subtract:
                desc.text = string.Format("-{0}", buffValue); break;
            case cfg.eventTrigger.type.multiply:
                desc.text = string.Format("×{0}", buffValue); break;
            case cfg.eventTrigger.type.divide:
                desc.text = string.Format("÷{0}", buffValue); break;
            default:
                desc.text = "0"; break;
        }
    }
}
