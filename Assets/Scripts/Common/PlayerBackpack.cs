using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{
    public event System.Action<ResourceType, int, int> OnPickupResource;
    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public Dictionary<ResourceType, int> Resources => resources;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PickUp(ResourceType type, int count)
    {
        if(!resources.ContainsKey(type))
        {
            resources.Add(type, 0);
        }
        OnPickupResource?.Invoke(type, resources[type], count);
        resources[type] += count;
    }
}
