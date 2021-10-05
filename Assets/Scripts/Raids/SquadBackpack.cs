using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBackpack : MonoBehaviour
{
    public event System.Action<ResourceType, int, int> OnPickupResource;

   /*[SerializeField] int maxResources = 50;
     [SerializeField] int resourcesPerLevel = 5;
     [SerializeField] StatsType resourcesStat;*/

    ResourcesInfo resources = new ResourcesInfo();

    public ResourcesInfo Resources => resources;

    public bool IsFilled { get; set; }

    //public int MaxResources { get; set; }

    int totalResources = 0;

    private void Start()
    {
        //MaxResources = maxResources + (FindObjectOfType<StatsManager>().GetStatInfo(resourcesStat).level * resourcesPerLevel);
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
        resources.Add(type, count);

        //if (totalResources > 0)
        {
            OnPickupResource?.Invoke(type, resources.Count(type), count);
        }
    }


    public void UseSpot(int count)
    {
        //var takeCount = Mathf.Min(MaxResources - totalResources, count);
        //totalResources += takeCount;
        //if (totalResources >= MaxResources)
        //{
        //    IsFilled = true;
        //}
    }
}
