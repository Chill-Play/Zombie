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
    public void SetUserTransform(Transform otherTransform)
    {
        user = otherTransform;
    }

    public void SpawnResources(ResourceType resourceType)
    {
        Resource instance = Instantiate(resourceType.defaultPrefab, transform.position, Quaternion.identity);
        instance.Pickup(user);
        Rigidbody body = instance.GetComponent<Rigidbody>();
        body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
        body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
        HideAllResources();
    }
}