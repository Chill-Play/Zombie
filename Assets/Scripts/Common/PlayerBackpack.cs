using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : SingletonMono<PlayerBackpack>, IComboCounter
{
    public event System.Action<ResourceType, int, int> OnPickupResource;

    public event Action<Sprite, int> OnAddingPoints;

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public Dictionary<ResourceType, int> Resources => resources;

    public void PickUp(ResourceType type, int count)
    {           
        if (!resources.ContainsKey(type))
        {
            resources.Add(type, 0);
        }
        resources[type] += count;   
        OnPickupResource?.Invoke(type, resources[type], count);
        OnAddingPoints?.Invoke(type.icon, count);
    }
}
