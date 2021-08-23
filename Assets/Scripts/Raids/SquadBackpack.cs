using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBackpack : MonoBehaviour
{
    public event System.Action<ResourceType, int, int> OnPickupResource;

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public Dictionary<ResourceType, int> Resources => resources;

    private void Start()
    {
        PlayerBackpack backpack = FindObjectOfType<PlayerBackpack>();
        backpack.OnPickupResource += Backpack_OnPickupResource;

        Squad squad = FindObjectOfType<Squad>();
        squad.OnUnitAdd += Squad_OnUnitAdd;
    }


    private void Squad_OnUnitAdd(Unit unit)
    {
        unit.GetComponent<PlayerBackpack>().OnPickupResource += Backpack_OnPickupResource;
    }

    public void Backpack_OnPickupResource(ResourceType type, int totalCount, int count)
    {
        if (!resources.ContainsKey(type))
        {
            resources.Add(type, 0);
        }
        resources[type] += count;
        OnPickupResource?.Invoke(type, resources[type], count);
    }
}
