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
        for (int i = 0; i < useSpots.Length; i++)
        {
            if (useSpots[i] != null)
            {
                Repairable target = useSpots[i].GetComponent<Repairable>();
                if (target != null && target.CanRepair)
                {
                    target.Repair(repairValue);
                    OnAddingPoints?.Invoke(icon, (int)repairValue);
                    return true;
                }
            }
        }
        return false;
    }
}
