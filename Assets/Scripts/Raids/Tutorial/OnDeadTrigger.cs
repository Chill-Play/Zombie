using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeadTrigger : ConditionTrigger

{
    [SerializeField] List<UnitHealth> unitHealths = new List<UnitHealth>();

    int unitsCount = 0;

    private void Awake()
    {
        for (int i = 0; i < unitHealths.Count; i++)
        {
            unitHealths[i].OnDead += OnDeadTrigger_OnDead;
            unitsCount = unitHealths.Count;
        }
    }

    private void OnDeadTrigger_OnDead(EventMessage<Empty> obj)
    {
        unitsCount--;
        if (unitsCount == 0)
        {
            InvokeEvent();
            unitHealths.Clear();
        }
    }
}
