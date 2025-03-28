using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class ResourcesController : SingletonMono<ResourcesController>
{
    public event System.Action OnResourcesUpdated;

    const string RESOURCE_PREFS = "M_Resource_";

    [SerializeField] List<ResourceType> resourceTypes;

    ResourcesInfo resourcesCount = new ResourcesInfo();
    public List<ResourceType> Resources => resourceTypes;
    public ResourcesInfo ResourcesCount => resourcesCount;

    void OnEnable()
    {
        resourcesCount.Clear();
        foreach(ResourceType resourceType in resourceTypes)
        {
            resourcesCount.AddSlot(resourceType, PlayerPrefs.GetInt(RESOURCE_PREFS + resourceType.saveId, 0));
        }
        UpdateResources();
    }



    public void UpdateResources()
    {
        OnResourcesUpdated?.Invoke();
        Save();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            foreach(var slot in resourcesCount.Slots)
            {
                slot.count += 100;
            }
            UpdateResources();
        }
    }


    public void AddResources(ResourcesInfo resources)
    {
        resourcesCount.Add(resources);
    }


    public ResourceType GetResourceType(string saveId)
    {
        return resourceTypes.FirstOrDefault((x) => x.saveId == saveId);
    }

    public void Save()
    {
        foreach (var slot in resourcesCount.Slots)
        {
            PlayerPrefs.SetInt(RESOURCE_PREFS + slot.type.saveId, slot.count);
        }
    }
}
