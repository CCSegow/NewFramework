using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using ZFramework.Core;

public class CharacterTable: DataToolBar<CharacterData>
{
    [FormerlySerializedAs("allCharecters")]
    [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
    private readonly List<CharacterWrapper> allCharacters;

    public CharacterData this[int index]
    {
        get { return this.allCharacters[index].Character; }
    }

    public CharacterTable(IEnumerable<CharacterData> characters,string path)
    {
        this.allCharacters = characters.Select(x => new CharacterWrapper(x)).ToList();
        SetPath(path);
    }

    private class CharacterWrapper
    {
        private CharacterData character; // Character is a ScriptableObject and would render a unity object
                                     // field if drawn in the inspector, which is not what we want.

        public CharacterData Character
        {
            get { return this.character; }
        }

        public CharacterWrapper(CharacterData character)
        {
            this.character = character;
        }

        [TableColumnWidth(50, false)]
        [ShowInInspector, PreviewField(45, ObjectFieldAlignment.Center)]
        public Sprite Icon { get { return this.character.Face; } set { this.character.Face = value; EditorUtility.SetDirty(this.character); } }

       
        [TableColumnWidth(120)]
        [ShowInInspector]
        public string Name { get { return this.character.Name; } set { this.character.Name = value; EditorUtility.SetDirty(this.character); } }

        [ShowInInspector, MinMaxSlider(1,100,true)]
        public Vector2 Level { get { return new Vector2(this.character.InitialLv, this.character.MaxLev); }  set { this.character.InitialLv = (int)value.x; this.character.MaxLev = (int)value.y; } }
        /*
       [ShowInInspector, ProgressBar(0, 100)]
       public float Shooting { get { return this.character.Skills.Shooting; } set { this.character.Skills.Shooting = value; EditorUtility.SetDirty(this.character); } }

       [ShowInInspector, ProgressBar(0, 100)]
       public float Melee { get { return this.character.Skills.Melee; } set { this.character.Skills.Melee = value; EditorUtility.SetDirty(this.character); } }

       [ShowInInspector, ProgressBar(0, 100)]
       public float Social { get { return this.character.Skills.Social; } set { this.character.Skills.Social = value; EditorUtility.SetDirty(this.character); } }

       [ShowInInspector, ProgressBar(0, 100)]
       public float Animals { get { return this.character.Skills.Animals; } set { this.character.Skills.Animals = value; EditorUtility.SetDirty(this.character); } }

       [ShowInInspector, ProgressBar(0, 100)]
       public float Medicine { get { return this.character.Skills.Medicine; } set { this.character.Skills.Medicine = value; EditorUtility.SetDirty(this.character); } }

       [ShowInInspector, ProgressBar(0, 100)]
       public float Crafting { get { return this.character.Skills.Crafting; } set { this.character.Skills.Crafting = value; EditorUtility.SetDirty(this.character); } }

       */
    }
}