using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
    [MenuItem("MyMenu/RemoveMissing")]
    static void SelectMissing(MenuCommand command)
    {
        Transform[] ts = Object.FindObjectsOfType<Transform>(true);
        List<GameObject> selection = new List<GameObject>();
        foreach (Transform t in ts)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(t.gameObject);
        }
    }
}
