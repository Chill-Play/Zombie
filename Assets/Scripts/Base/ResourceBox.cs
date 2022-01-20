using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
struct ResourceData
{
    public GameObject gameObject;
    public ResourceType resourceType;
}

public class ResourceBox : MonoBehaviour
{
    [SerializeField] private ResourceData[] resources;
    [SerializeField] private float resourcesVelocity;
    [SerializeField] private int spawnLimit = 20;
    private Transform user;
    
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
        GameObject go = FindResource(resourceType);
        if (go != null)
        {
            go.SetActive(true);
        }
    }

    public void HideResource(ResourceType resourceType)
    {
        GameObject go = FindResource(resourceType);
        if (go != null)
        {
            go.SetActive(false);
        }
    }

    public void HideAllResources()
    {
        foreach (var resource in resources)
        {
            resource.gameObject.SetActive(false);
        }
    }
    public void SetUserTransform(Transform otherTransform)
    {
        user = otherTransform;
    }

    void SpawnResource(ResourceType resourceType, int count)
    {
        Resource instance = Instantiate(resourceType.defaultPrefab, transform.position, Quaternion.identity);
        instance.SetCount(count);
        instance.Pickup(user);
        Rigidbody body = instance.GetComponent<Rigidbody>();
        body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) *
                        resourcesVelocity;
        body.angularVelocity =
            new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
    }
    
    public void SpawnResources(ResourceType resourceType, int resourceValue)
    {
        int spawnCount = Mathf.Min(resourceValue, spawnLimit);
        int resCount = resourceValue / spawnCount;
        for (int i = 0; i < spawnCount - 1; i++)
            SpawnResource(resourceType, resCount);
        SpawnResource(resourceType, resourceValue - resCount * (spawnCount - 1));
        HideAllResources();
        
    }
}