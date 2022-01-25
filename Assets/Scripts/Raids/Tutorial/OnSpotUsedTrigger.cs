using System.Collections;
using System.Collections.Generic;
using GooglePlayServices;
using UnityEngine;

public class OnSpotUsedTrigger : ConditionTrigger
{
    [SerializeField] List<ResourceSpot> resourceSpots = new List<ResourceSpot>();
    [SerializeField] private Collider collider;

    int resourceSpotsCount = 0;

    private void Start()
    {
        for (int i = 0; i < resourceSpots.Count; i++)
        {
            resourceSpots[i].OnSpotUsed += OnSpotUsedTrigger_OnSpotUsed; 
        }
        resourceSpotsCount = resourceSpots.Count;
    }

    private void OnSpotUsedTrigger_OnSpotUsed(ResourceSpot obj)
    {

        collider.enabled = false;
        resourceSpotsCount--;
        if (resourceSpotsCount == 0)
        {
            InvokeEvent();
            resourceSpots.Clear();
        }
    }
}
