using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class ResourceFactory : BaseObject, IUnloadingResources
{
    [SerializeField] TMP_Text countText;
    [SerializeField] Upgradable upgradable;
    [SerializeField] ResourceType resourceType;
    [SerializeField] protected float workTime = 300f;
    [SerializeField] protected float fullProductionTime = 300f;
    [SerializeField] protected int baseResourcesLimit = 30;
    [SerializeField] protected int resourcesLimitPerLevel = 10;

    [BaseSerialize] protected int currentResourcesCount;
    [BaseSerialize] protected string lastResourcesUpdate;
    float nextResourceTime;
    bool working = false;
    protected float productionTime;
    protected int resourcesLimit;

    public ResourceType ResourcesType => resourceType;

    public int CurrentCount => currentResourcesCount;

    private void Awake()
    {
        upgradable.OnLevelUp += Upgradable_OnLevelUp;
    }


    protected virtual void Start()
    {
        resourcesLimit = baseResourcesLimit + resourcesLimitPerLevel * upgradable.Level;
        productionTime = resourcesLimit / fullProductionTime;
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

    private void Upgradable_OnLevelUp()
    {
        resourcesLimit = baseResourcesLimit + resourcesLimitPerLevel * upgradable.Level;
        productionTime = resourcesLimit / fullProductionTime;
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

        transform.DOPunchScale(new Vector3(1,1,1),1);
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

    //public virtual void Show
}
