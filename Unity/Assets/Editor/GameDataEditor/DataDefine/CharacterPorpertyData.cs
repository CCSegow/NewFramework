using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
public class CharacterPorpertyData : ScriptableObject
{
    public string Name;
    public CharacterProperty CharacterProperty;
}
[System.Serializable]
public struct CharacterProperty
{
    public AnimationCurve Hp;
    public AnimationCurve Atk;
    public AnimationCurve Def;
    public AnimationCurve Vit;
    public AnimationCurve AtkSpeed;
    public AnimationCurve MoveSpeed;
}

public struct GrowingCurve { 

}