using UnityEngine;
using System.Collections;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using System;

public class DateTimeDrawer : OdinValueDrawer<DateTime>
{
    
    protected override void DrawPropertyLayout(GUIContent label)
    {

        SirenixEditorFields.DelayedTextField(label, this.ValueEntry.SmartValue.ToString());

    }
}
