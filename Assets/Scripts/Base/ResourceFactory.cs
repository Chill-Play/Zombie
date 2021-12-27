using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResourceFactory : BaseObject, IUnloadingResources
{
    [SerializeField] TMP_Text countText;
    [SerializeField] ResourceType resourceType;
    [SerializeField] protected float workTime = 300f;
    [SerializeField] protected float productionTime = 300f;
    [SerializeField] protected int resourcesLimit = 500;

    [BaseSerialize] protected int currentResourcesCount;
    [BaseSerialize] protected string lastResourcesUpdate;
    float nextResourceTime;
    bool working = false;

    public ResourceType ResourcesType => resourceType;

    public int CurrentCount => currentResourcesCount;

    protected virtual void Start()
    {
        if (!string.IsNullOrEmpty(lastResourcesUpdate))
        {
            DateTime dateTime = DateTime.FromBinary(Convert.ToInt64(lastResourcesUpdate));
            TimeSpan delta = DateTime.UtcNow - dateTime;
            int deltaCount = (int)(delta.TotalSeconds / productionTime);
            AddResource(deltaCount);
        }
        nextResourceTime = Time.time + productionTime;
        UpdateCountText();
        if (currentResourcesCount < resourcesLimit)
        {
            StartWork();
        }
        else
        {
            StopWork();
        }
    }

    protected virtual void Update()
    {
        if (nextResourceTime < Time.time && currentResourcesCount < resourcesLimit)
        {
            AddResource();
            nextResourceTime = Time.time + productionTime;          
        }
    }

    protected virtual void AddResource(int count = 1)
    {
        currentResourcesCount = Mathf.Clamp(currentResourcesCount + count, 0, resourcesLimit);
        UpdateCountText();
        if (currentResourcesCount == resourcesLimit)
        {
            StopWork();
        }
        lastResourcesUpdate = DateTime.UtcNow.ToBinary().ToString();
    }

    void UpdateCountText()
    {
        countText.text = currentResourcesCount.ToString() + "/" + resourcesLimit.ToString();
    }

    protected virtual void StartWork()
    {
        working = true;
    }


    protected virtual void StopWork()
    {
        working = false;
    }

    public virtual void Unload(int count)
    {
        currentResourcesCount = Mathf.Clamp(currentResourcesCount - count, 0, resourcesLimit);
        UpdateCountText();
        if (!working)
        {
            StartWork();
        }
        lastResourcesUpdate = DateTime.UtcNow.ToBinary().ToString();
    }
}
