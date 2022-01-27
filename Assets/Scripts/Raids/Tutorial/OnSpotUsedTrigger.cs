using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSpotUsedTrigger : ConditionTrigger
{

    [SerializeField] List<ResourceSpot> resourceSpots = new List<ResourceSpot>();
    [SerializeField] private Collider[] colliders;

    int resourceSpotsCount = 0;

    private void Start()
    {
        for (int i = 0; i < resourceSpots.Count; i++)
        {
            resourceSpots[i].OnSpotUsed += OnSpotUsedTrigger_OnSpotUsed; 
        }
        resourceSpotsCount = resourceSpots.Count;
        foreach (var collider in colliders)
            collider.enabled = false;
    }

    private void OnSpotUsedTrigger_OnSpotUsed(ResourceSpot obj)
    {
        foreach (var collider in colliders)
            collider.enabled = true;
        resourceSpotsCount--;
        if (resourceSpotsCount == 0)
        {
            InvokeEvent();
            resourceSpots.Clear();
        }
    }
}
