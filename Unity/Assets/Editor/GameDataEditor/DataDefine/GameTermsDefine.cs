using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

[Serializable]
public class TermData 
{
    [HorizontalGroup("术语"),HideLabel]
    public string Name;
    [HorizontalGroup("描述"), HideLabel]
    public string Describe;

    [HorizontalGroup("中文"), HideLabel]
    public string Chinese;
    [HorizontalGroup("繁体中文"), HideLabel]
    public string Traditional_Chinese;
    [HorizontalGroup("英文"), HideLabel]
    public string English;
    [HorizontalGroup("日文"), HideLabel]
    public string Japanese;

         
}
public class GameTermsDefine :ScriptableObject{

    public string Name;

    [TableList,LabelText("$Name")]
    public List<TermData> Stats;
}