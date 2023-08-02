using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
namespace ZFramework.Core
{
    public class CharacterData : ScriptableObject
    {
        [HorizontalGroup("Base", LabelWidth = 180)]
        [VerticalGroup("Base/Left")]
        [HorizontalGroup("Base/Left/Name"), LabelText("角色名称")]
        public string Name;
        [HorizontalGroup("Base/Left/Name"), LabelText("其他称呼")]
        public string NickName;
        [HorizontalGroup("Base/Left/Lv"), LabelText("初始等级")]
        public int InitialLv = 1;
        [HorizontalGroup("Base/Left/Lv"), LabelText("最大等级")]
        public int MaxLev = 99;

        //[BoxGroup("Base/Left/Outlook")]
        [HorizontalGroup("Base/Left/Outlook"), InlineEditor(InlineEditorModes.LargePreview), LabelText("立绘")]
        public Sprite Photo;
        [HorizontalGroup("Base/Left/Outlook"), InlineEditor(InlineEditorModes.SmallPreview), LabelText("头像")]
        public Sprite Face;
        [BoxGroup("Base/Left/模型"), ShowInInspector, HideLabel]
        public CharacterBind CharacterBind = new CharacterBind();

        [VerticalGroup("Base/Right")]
        [BoxGroup("Base/Right/特性"), ShowInInspector, HideLabel, ListDrawerSettings]
        public List<Trait> Traits = new List<Trait>();

        [BoxGroup("Base/Right/备注")]
        [TextArea(5, 10), HideLabel]
        public string Note;
    }
    [Serializable]
    public struct Trait
    {
        public string Name;
    }
    [Serializable]
    public class CharacterBind
    {
        [InlineEditor(InlineEditorModes.LargePreview)]
        public GameObject Prefab;

        public GameObject Head;
        public GameObject Body;
        public GameObject LeftHand;
        public GameObject RightHand;
        public GameObject Foot;
    }
}