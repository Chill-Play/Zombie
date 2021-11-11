using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRepairing : UnitInstrument
{
    [SerializeField] float repairValue = 10f;

    protected override bool Use()
    {
        base.Use();
        Repairable target = useSpots[0].GetComponent<Repairable>();
        if (target != null && target.CanRepair)
        {
            target.Repair(repairValue);
            return true;
        }
        return false;
    }
}
