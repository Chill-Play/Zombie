using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadingArea : MonoBehaviour
{
    [SerializeField] Transform unloadingPoint;
    [SerializeField] GameObject unloadingTarget;
    [SerializeField] float baseRate = 0.25f;
    [SerializeField] float rateIncrease = 0.01f;
    [SerializeField] float resourcesVelocity = 2f;
    UINumbers uiNumbers;

    IUnloadingResources unloadingResources;
    UnitInteracting unitInteracting;

    float nextUse;
    int uses;

    private void Awake()
    {
        unloadingResources = unloadingTarget.GetComponent<IUnloadingResources>(); //refactor
        uiNumbers = FindObjectOfType<UINumbers>();
    }

    private void Update()
    {
        if (unitInteracting != null && unitInteracting.enabled)
        {
            if (nextUse < Time.time)
            {
                if (unloadingResources.CurrentCount > 0)
                {
                    int count = (int)Mathf.Clamp(unloadingResources.MaxCount * 0.1f, 1, unloadingResources.CurrentCount);
                    unloadingResources.Unload(count);
                    SpawnResources(unitInteracting.transform, count);
                    uses++;
                    nextUse = Time.time + baseRate - (rateIncrease * uses);
                }
            }
        }
        else
        {
            uses = 0;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        unitInteracting = collider.GetComponent<UnitInteracting>();     
    }

    private void OnTriggerExit(Collider collider)
    {
        UnitInteracting unitInteracting = collider.GetComponent<UnitInteracting>();
        if (this.unitInteracting == unitInteracting)
        {
            this.unitInteracting = null;
        }
    }

    void SpawnResources(Transform user, int count)
    {
        ResourceType resourceType = unloadingResources.ResourcesType;
        Resource instance = Instantiate(unloadingResources.ResourcesType.defaultPrefab, unloadingPoint.position, Quaternion.identity);
        instance.Pickup(user.transform);
        instance.SetCount(count);
        Rigidbody body = instance.GetComponent<Rigidbody>();
        body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
        body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
        uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "+" + count, Vector2.zero, 15f, 10f, 0.6f, resourceType.icon);
    }

}
