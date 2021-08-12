using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildable : BaseObject
{
    public event System.Action OnUpdate;
    public event System.Action OnBuilt;

    [SerializeField] ResourcesInfo cost;
    [BaseSerialize] ResourcesInfo resourcesSpent = new ResourcesInfo();

    public ResourcesInfo Cost => cost;
    public ResourcesInfo ResourcesSpent => resourcesSpent;

    public bool Built { get; set; }


    public void Awake()
    {
        if(resourcesSpent.Slots.Count < cost.Slots.Count)
        {
            resourcesSpent.ApplyTypes(cost);
        }
    }


    public void SpendResources(ResourcesInfo info, int count)
    {
        if (Built) return;
        resourcesSpent.Spend(info, cost, count);
        if(cost.IsFilled(resourcesSpent))
        {
            Built = true;
            enabled = false;
            OnBuilt?.Invoke();
        }
        else
        {
            OnUpdate?.Invoke();
        }
    }
}
