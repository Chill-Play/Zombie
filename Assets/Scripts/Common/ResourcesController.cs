using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleJSON;




public class ResourcesController : SingletonMono<ResourcesController>
{
    public event System.Action OnResourcesUpdated;

    const string RESOURCE_PREFS = "M_Resource_";

    [SerializeField] List<ResourceType> resourceTypes;
    [SerializeField] List<ResourceType> defaultResourceTypes;
    [SerializeField] ResourceType defaultResourceType;

    List<ResourceType> openedResources = new List<ResourceType>();

    ResourcesInfo resourcesCount = new ResourcesInfo();
    public List<ResourceType> Resources => resourceTypes;
    public ResourcesInfo ResourcesCount => resourcesCount;

    public ResourceType DefaultResourceType => defaultResourceType;
    public List<ResourceType> OpenedResources => openedResources;

    void OnEnable()
    {
        resourcesCount.Clear();
        foreach(ResourceType resourceType in resourceTypes)
        {
            var jsonObject = JSON.Parse(PlayerPrefs.GetString(RESOURCE_PREFS + resourceType.saveId, ""));
            if (jsonObject.HasKey("count"))
            {
                int count = jsonObject["count"].AsInt;
                bool opened = jsonObject["opened"].AsBool;
                ResourceSlot resourceSlot = new ResourceSlot(resourceType, count, opened);
                resourcesCount.AddSlot(resourceSlot);
                if (opened)
                {
                openedResources.Add(resourceType);
                }
            }
            else if (defaultResourceTypes.Contains(resourceType))
            {
                ResourceSlot resourceSlot = new ResourceSlot(resourceType, 0, true);
                resourcesCount.AddSlot(resourceSlot);
                openedResources.Add(resourceType);
            }            
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

    public void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,200,200), "Add Resources"))
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

    public void AddResources(ResourceType resourceType, int count)
    {
        resourcesCount.Add(resourceType, count);
    }

    public void AddResourceType(ResourceType resourceType)
    {
        if (!openedResources.Contains(resourceType))
        {
            ResourceSlot resourceSlot = new ResourceSlot(resourceType, 0, true);
            resourcesCount.AddSlot(resourceSlot);
            openedResources.Add(resourceType);
        }
    }


    public ResourceType GetResourceType(string saveId)
    {
        return resourceTypes.FirstOrDefault((x) => x.saveId == saveId);
    }

    public void Save()
    {
        foreach (var slot in resourcesCount.Slots)
        {
            JSONObject jsonObject = new JSONObject();
            jsonObject.Add("count", slot.count);
            jsonObject.Add("opened", slot.opened);
            PlayerPrefs.SetString(RESOURCE_PREFS + slot.type.saveId, jsonObject.ToString());
        }
    }
}
