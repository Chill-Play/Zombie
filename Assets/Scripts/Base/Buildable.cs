using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildable : BaseObject
{
    public event System.Action OnUpdate;
    public event System.Action<bool> OnBuilt; // true if built after deserialization 

    [SerializeField] ResourcesInfo cost;
    [BaseSerialize] ResourcesInfo resourcesSpent = new ResourcesInfo();
    [BaseSerialize] bool built;

    public ResourcesInfo Cost => cost;
    public ResourcesInfo ResourcesSpent => resourcesSpent;

    public bool Built => built;


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
            FinishBuilding(false);
        }
        else
        {
            OnUpdate?.Invoke();
        }
    }

    private void FinishBuilding(bool afterDeserialization)
    {
        built = true;
        enabled = false;
        OnBuilt?.Invoke(afterDeserialization);
    }

    public override void BaseAfterDeserialize()
    {
        base.BaseAfterDeserialize();
        if (built)
        {
            FinishBuilding(true);
        }
    }
}
