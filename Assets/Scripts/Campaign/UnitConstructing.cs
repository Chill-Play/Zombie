using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConstructing : UnitInstrument
{
    [SerializeField] float constructValue = 10f;

    protected override bool Use()
    {
        base.Use();
        Constructive target = useSpots[0].GetComponent<Constructive>();
        if (target != null && target.CanConstruct)
        {
            target.Construct(constructValue);
            return true;
        }
        return false;
    }

}
