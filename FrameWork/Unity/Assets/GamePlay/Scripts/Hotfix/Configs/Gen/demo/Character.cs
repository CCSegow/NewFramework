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

public sealed partial class Character :  Bright.Config.BeanBase 
{
    public Character(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["path"].IsString) { throw new SerializationException(); }  Path = _json["path"]; }
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        { if(!_json["total_hp"].IsNumber) { throw new SerializationException(); }  TotalHp = _json["total_hp"]; }
        PostInit();
    }

    public Character(int id, string path, string name, string desc, int total_hp ) 
    {
        this.Id = id;
        this.Path = path;
        this.Name = name;
        this.Desc = desc;
        this.TotalHp = total_hp;
        PostInit();
    }

    public static Character DeserializeCharacter(JSONNode _json)
    {
        return new demo.Character(_json);
    }

    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// 角色模型路径
    /// </summary>
    public string Path { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 队伍总血量
    /// </summary>
    public int TotalHp { get; private set; }

    public const int __ID__ = -469668674;
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
        + "Path:" + Path + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "TotalHp:" + TotalHp + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}