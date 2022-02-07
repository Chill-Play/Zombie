using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBackpack : SingletonMono<SquadBackpack>
{
    public event System.Action<ResourceType, int, int> OnPickupResource;

    ResourcesInfo resources = new ResourcesInfo();

    public ResourcesInfo Resources => resources;

    public bool IsFilled { get; set; }

    int totalResources = 0;

    private void Awake()
    {
        Squad squad = Squad.Instance;
        squad.Units[0].GetComponent<PlayerBackpack>().OnPickupResource += Backpack_OnPickupResource;
        squad.OnUnitAdd += Squad_OnUnitAdd;
    }

    private void Squad_OnUnitAdd(Unit unit)
    {
        if (unit.TryGetComponent<PlayerBackpack>(out PlayerBackpack playerBackpack))
        {
            playerBackpack.OnPickupResource += Backpack_OnPickupResource;
        }
    }

    public void Backpack_OnPickupResource(ResourceType type, int totalCount, int count)
    {
        resources.Add(type, count);      
        OnPickupResource?.Invoke(type, resources.Count(type), count);        
    }
}
