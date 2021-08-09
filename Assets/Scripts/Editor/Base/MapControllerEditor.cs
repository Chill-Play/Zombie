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
            for (int i = 0; i < myTarget.MapCells.Count; i++)
            {
                EditorUtility.SetDirty(myTarget.MapCells[i]);
            }
            for (int i = 0; i < myTarget.Buildings.Count; i++)
            {
                EditorUtility.SetDirty(myTarget.Buildings[i]);
            }
        }

        if (GUILayout.Button("Clear"))
        {
            myTarget.ClearPrefs();
            EditorUtility.SetDirty(myTarget);
            for (int i = 0; i < myTarget.MapCells.Count; i++)
            {
                EditorUtility.SetDirty(myTarget.MapCells[i]);
            }
            for (int i = 0; i < myTarget.Buildings.Count; i++)
            {
                EditorUtility.SetDirty(myTarget.Buildings[i]);
            }
        }
    }

}
