using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBuildTrigger : ConditionTrigger
{
    [SerializeField] List<Buildable> buildables = new List<Buildable>();

    int buildableCount = 0;

    private void Start()
    {
        for (int i = 0; i < buildables.Count; i++)
        {
            buildables[i].OnBuilt += OnBuildTrigger_OnBuilt; 
        }
        buildableCount = buildables.Count;
    }

    private void OnBuildTrigger_OnBuilt(bool obj)
    {
        buildableCount--;
        if (buildableCount == 0)
        {
            InvokeEvent();
            buildables.Clear();
        }
       
    }
}
