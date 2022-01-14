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

    protected override void Use(Collider target)
    {
        base.Use(target);
        Repairable repairableTarget = target.GetComponent<Repairable>();
        repairableTarget.Repair(repairValue);
        OnAddingPoints?.Invoke(icon, (int)repairValue);
    }

    protected override bool CanUse(Collider useSpot)
    {
        Repairable target = useSpot.GetComponent<Repairable>();
        if (target != null && target.CanRepair)
        {
            return true;
        }
        return false;
    }

    public void AddRepairingPower(float value)
    {
        repairValue += value;
    }
}
