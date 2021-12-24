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
        icon = FindObjectOfType<ConstructionManager>().ConstructionIcon;
    }

    protected override bool Use()
    {
        base.Use();   
        for (int i = 0; i < useSpots.Length; i++)
        {
            if (useSpots[i] != null)
            {
                Constructive target = useSpots[i].GetComponent<Constructive>();
                if (target != null && target.CanConstruct)
                {
                    target.Construct(constructValue);
                    OnAddingPoints?.Invoke(icon, (int)constructValue);
                    return true;
                }
            }
        }
        return false;
    }

}
