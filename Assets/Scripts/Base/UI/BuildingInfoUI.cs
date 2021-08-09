using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoUI : MonoBehaviour
{
    [SerializeField] Building building;
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] Transform resourcesLayout;

    Dictionary<ResourceType, ResourceBar> resourceBars = new Dictionary<ResourceType, ResourceBar>();

    void OnEnable()
    {
        building.OnBuildingInited += Building_OnBuildingInited;
        building.OnUpdate += Building_OnUpdate;
    }


    private void OnDisable()
    {
        building.OnBuildingInited -= Building_OnBuildingInited;
        building.OnUpdate -= Building_OnUpdate;
    }


    void Update()
    {

    }


    private void Building_OnBuildingInited()
    {
        foreach (var cost in building.Cost)
        {
            ResourceBar bar = Instantiate(resourceBarPrefab, resourcesLayout);
            bar.Setup(cost.type, cost.count);
            resourceBars.Add(cost.type, bar);
        }
    }


    private void Building_OnUpdate()
    {
        foreach (var pair in building.Resources)
        {
            resourceBars[pair.Key].UpdateValue(pair.Value);
            if (pair.Value == 0)
            {
                resourceBars[pair.Key].gameObject.SetActive(false);
            }
        }
    }
}
