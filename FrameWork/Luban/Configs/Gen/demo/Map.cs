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



namespace cfg.demo
{ 

public sealed partial class Map :  Bright.Config.BeanBase 
{
    public Map(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["start_map"].IsNumber) { throw new SerializationException(); }  StartMap = _json["start_map"]; }
        { if(!_json["end_map"].IsNumber) { throw new SerializationException(); }  EndMap = _json["end_map"]; }
        { var __json0 = _json["loop_map"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; LoopMap = new int[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  LoopMap[__index0++] = __v0; }   }
        { if(!_json["loop_num"].IsNumber) { throw new SerializationException(); }  LoopNum = _json["loop_num"]; }
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        { var __json0 = _json["event_id"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; EventId = new int[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  EventId[__index0++] = __v0; }   }
        PostInit();
    }

    public Map(int id, int start_map, int end_map, int[] loop_map, int loop_num, string name, string desc, int[] event_id ) 
    {
        this.Id = id;
        this.StartMap = start_map;
        this.EndMap = end_map;
        this.LoopMap = loop_map;
        this.LoopNum = loop_num;
        this.Name = name;
        this.Desc = desc;
        this.EventId = event_id;
        PostInit();
    }

    public static Map DeserializeMap(JSONNode _json)
    {
        return new demo.Map(_json);
    }

    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 起始地图块id
    /// </summary>
    public int StartMap { get; private set; }
    /// <summary>
    /// 终点地图块id
    /// </summary>
    public int EndMap { get; private set; }
    /// <summary>
    /// 场景内循环地图块id
    /// </summary>
    public int[] LoopMap { get; private set; }
    /// <summary>
    /// 循环地图块数量
    /// </summary>
    public int LoopNum { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 事件组
    /// </summary>
    public int[] EventId { get; private set; }

    public const int __ID__ = 855468721;
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
        + "Id:" + Id + ","
        + "StartMap:" + StartMap + ","
        + "EndMap:" + EndMap + ","
        + "LoopMap:" + Bright.Common.StringUtil.CollectionToString(LoopMap) + ","
        + "LoopNum:" + LoopNum + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "EventId:" + Bright.Common.StringUtil.CollectionToString(EventId) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}