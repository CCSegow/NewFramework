//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg.item
{ 

/// <summary>
/// 商店礼包
/// </summary>
public sealed partial class ItemPackage :  Bright.Config.BeanBase 
{
    public ItemPackage(JSONNode _json) 
    {
        { if(!_json["itemId"].IsNumber) { throw new SerializationException(); }  ItemId = _json["itemId"]; }
        { if(!_json["num"].IsNumber) { throw new SerializationException(); }  Num = _json["num"]; }
        PostInit();
    }

    public ItemPackage(int itemId, int num ) 
    {
        this.ItemId = itemId;
        this.Num = num;
        PostInit();
    }

    public static ItemPackage DeserializeItemPackage(JSONNode _json)
    {
        return new item.ItemPackage(_json);
    }

    /// <summary>
    /// 物品id
    /// </summary>
    public int ItemId { get; private set; }
    /// <summary>
    /// 物品数量
    /// </summary>
    public int Num { get; private set; }

    public const int __ID__ = 1542593336;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "ItemId:" + ItemId + ","
        + "Num:" + Num + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
