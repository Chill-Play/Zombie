using System;
using UnityEngine;

[Serializable]
public struct Resources
{
    public GameObject gameObject;
    public ResourceType resourceType;
}

public class ResourceBox : MonoBehaviour
{
    [SerializeField] private Resources[] resources;

    GameObject FindResource(ResourceType resourceType)
    {
        foreach (var resource in resources)
        {
            if (resource.resourceType == resourceType)
                return resource.gameObject;
        }
        return null;
    }
    
    public void ShowResource(ResourceType resourceType)
    {
        FindResource(resourceType).SetActive(true);
    }

    public void HideResource(ResourceType resourceType)
    {
        FindResource(resourceType).SetActive(false);
    }

    public void HideAllResources()
    {
        foreach (var resource in resources)
        {
            resource.gameObject.SetActive(false);
        }
    }
}