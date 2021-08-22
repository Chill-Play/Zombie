using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{
    public event System.Action<ResourceType, int, int> OnPickupResource;  

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public Dictionary<ResourceType, int> Resources => resources;

    public void PickUp(ResourceType type, int count)
    {
        if (!resources.ContainsKey(type))
        {
            resources.Add(type, 0);
        }
        OnPickupResource?.Invoke(type, resources[type], count);
        resources[type] += count;   
    }
}
