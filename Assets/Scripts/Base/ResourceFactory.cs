using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class ResourceFactory : BaseObject, IUnloadingResources
{
    const int INTERSTITIONAL_UNLOAD_CONT = 5;

    [SerializeField] TMP_Text countText;
    [SerializeField] Upgradable upgradable;
    [SerializeField] ResourceType resourceType;
    [SerializeField] Buildable buildable;
    [SerializeField] protected float workTime = 300f;
    [SerializeField] protected float fullProductionTime = 300f;
    [SerializeField] protected int baseResourcesLimit = 30;
    [SerializeField] protected int resourcesLimitPerLevel = 10;
    [SerializeField] private Transform counter;

    [BaseSerialize] protected int currentResourcesCount;
    [BaseSerialize] protected string lastResourcesUpdate;
    float nextResourceTime;
    bool working = false;
    protected float productionTime;
    protected int resourcesLimit;

    public int MaxCount => resourcesLimit;
    public ResourceType ResourcesType => resourceType;

    public int CurrentCount => currentResourcesCount;

    private void Awake()
    {
        upgradable.OnLevelUp += Upgradable_OnLevelUp;
        buildable.OnBuilt += Buildable_OnBuilt;       
    }

    private void Start()
    {
        if (buildable.Built)
        {
            Setup();
        }
    }

    protected virtual void Setup()
    {
        resourcesLimit = baseResourcesLimit + resourcesLimitPerLevel * upgradable.Level;
        productionTime = fullProductionTime / resourcesLimit;        
        if (!string.IsNullOrEmpty(lastResourcesUpdate) && lastResourcesUpdate != "null")
        {
            DateTime dateTime = DateTime.FromBinary(Convert.ToInt64(lastResourcesUpdate));
            TimeSpan delta = DateTime.UtcNow - dateTime;
            int deltaCount = (int)(delta.TotalSeconds / productionTime);
            AddResource(deltaCount);
        }

        if (currentResourcesCount < resourcesLimit)
        {
            StartWork();
        }
        else
        {
            StopWork();
        }
        nextResourceTime = Time.time + productionTime;
        UpdateCountText();          
    }

    private void Buildable_OnBuilt(bool obj)
    {     
        Setup();
    }

    private void Upgradable_OnLevelUp()
    {
        resourcesLimit = baseResourcesLimit + resourcesLimitPerLevel * upgradable.Level;
        productionTime = fullProductionTime / resourcesLimit;
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

        counter.DOPunchScale(new Vector3(.3f,.3f,.3f),.5f, 8, 1);
    }

    protected virtual void Update()
    {
        if (working && nextResourceTime < Time.time && currentResourcesCount < resourcesLimit)
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
        int delta = Mathf.Clamp(count, 0, currentResourcesCount);
        currentResourcesCount = currentResourcesCount - delta;
        if (delta >= INTERSTITIONAL_UNLOAD_CONT)
        {
            AdvertisementManager.Instance.TryShowInterstitial("unload_resources");
        }
        UpdateCountText();
        if (!working)
        {
            StartWork();
        }
        lastResourcesUpdate = DateTime.UtcNow.ToBinary().ToString();
    }

    //public virtual void Show
}
