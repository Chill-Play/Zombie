using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapController))]
public class MapControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapController myTarget = (MapController)target;

        if (GUILayout.Button("ApplyChanges"))
        {
            myTarget.ApplyMapChanges();
            EditorUtility.SetDirty(myTarget);
        }
    }

}
