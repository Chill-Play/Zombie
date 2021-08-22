using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractivePoint))]
public class InteractivePointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InteractivePoint myTarget = (InteractivePoint)target;

        if (GUILayout.Button("ApplyChanges"))
        {
            EditorUtility.SetDirty(myTarget);
            myTarget.GenerateWorkingPoints();
        }
    }
}
