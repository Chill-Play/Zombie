using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResourceFactory : BaseObject
{
    [SerializeField] TMP_Text countText;
    [SerializeField] ResourceType resourceType;
    [SerializeField] protected float workTime = 300f;
    [SerializeField] protected float productionTime = 300f;
    [SerializeField] protected int resourcesLimit = 500;

    [BaseSerialize] protected int currentResourcesCount;
    [BaseSerialize] protected string lastResourcesUpdate;
    float nextResourceTime;

    protected virtual void Start()
    {
        if (!string.IsNullOrEmpty(lastResourcesUpdate))
        {
            DateTime dateTime = DateTime.FromBinary(Convert.ToInt64(lastResourcesUpdate));
            TimeSpan delta = DateTime.UtcNow - dateTime;
            int deltaCount = (int)(delta.TotalSeconds / productionTime);
            currentResourcesCount += deltaCount;
        }
        nextResourceTime = Time.time + productionTime;
        UpdateCountText();
    }

    protected virtual void Update()
    {
        if (nextResourceTime < Time.time && currentResourcesCount < resourcesLimit)
        {
            AddResource();
            nextResourceTime = Time.time + productionTime;
            lastResourcesUpdate = DateTime.UtcNow.ToBinary().ToString();           
        }
    }

    protected virtual void AddResource(int count = 1)
    {       
        currentResourcesCount += count;
        UpdateCountText();
    }

    void UpdateCountText()
    {
        countText.text = currentResourcesCount.ToString() + "/" + resourcesLimit.ToString();
    }

}
