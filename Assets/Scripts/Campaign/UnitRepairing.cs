using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRepairing : UnitInstrument, IComboCounter
{
    [SerializeField] float repairValue = 10f;

    public event Action<Sprite, int> OnAddingPoints;

    Sprite icon;

    protected override void Awake()
    {
        base.Awake();      
        icon = FindObjectOfType<ConstructionManager>().ConstructionIcon;
    }

    protected override bool Use()
    {
        base.Use();
        Repairable target = useSpots[0].GetComponent<Repairable>();
        if (target != null && target.CanRepair)
        {
            target.Repair(repairValue);
            OnAddingPoints?.Invoke(icon, (int)repairValue);
            return true;
        }
        return false;
    }
}
