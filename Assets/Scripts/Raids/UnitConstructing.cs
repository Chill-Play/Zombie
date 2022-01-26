using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConstructing : UnitInstrument, IComboCounter
{
    [SerializeField] float constructValue = 10f;

    public event Action<Sprite, int> OnAddingPoints;

    Sprite icon;

    protected override void Awake()
    {
        base.Awake();
        icon = ConstructionManager.Instance.ConstructionIcon;
    }

    protected override void Use(Collider target)
    {
        base.Use(target);
        Constructive constructiveTarget = target.GetComponent<Constructive>();
        constructiveTarget.Construct(constructValue);
        OnAddingPoints?.Invoke(icon, (int)constructValue);       
    }

    protected override bool CanUse(Collider useSpot)
    {
        Constructive target = useSpot.GetComponent<Constructive>();
        if (target != null && target.CanConstruct)
        {
            return true;
        }

        return false;   
    }

    public void AddConstructingPower(float value)
    {
        constructValue += value;
    }

}
