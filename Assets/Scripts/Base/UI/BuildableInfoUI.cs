using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableInfoUI : MonoBehaviour
{
    [SerializeField] Buildable building;
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] Transform resourcesLayout;

    Dictionary<ResourceType, ResourceBar> resourceBars = new Dictionary<ResourceType, ResourceBar>();

    void OnEnable()
    {
        building.OnUpdate += Building_OnUpdate;
        building.OnBuilt += Building_OnBuilt;
        foreach (var cost in building.Cost.Slots)
        {
            ResourceBar bar = Instantiate(resourceBarPrefab, resourcesLayout);
            bar.Setup(cost.type, cost.count);
            resourceBars.Add(cost.type, bar);
        }
    }


    private void OnDisable()
    {
        building.OnBuilt -= Building_OnBuilt;
        building.OnUpdate -= Building_OnUpdate;
    }


    void Update()
    {

    }


    private void Building_OnBuildingInited()
    {
        foreach (var cost in building.Cost.Slots)
        {
            ResourceBar bar = Instantiate(resourceBarPrefab, resourcesLayout);
            bar.Setup(cost.type, cost.count);
            resourceBars.Add(cost.type, bar);
        }
    }


    private void Building_OnUpdate()
    {
        foreach (var slot in building.ResourcesSpent.Slots)
        {
            foreach (var costSlot in building.Cost.Slots)
            {
                if (costSlot.type == slot.type)
                {
                    var bar = resourceBars[slot.type];
                    var value = costSlot.count - slot.count;
                    bar.UpdateValue(value);
                    if (value == 0)
                    {
                        bar.transform.DOScale(0f, 0.5f).SetEase(Ease.InSine).OnComplete(() => bar.gameObject.SetActive(false));
                    }
                }
            }
        }
    }


    private void Building_OnBuilt()
    {
        foreach(var pair in resourceBars)
        {
            pair.Value.UpdateValue(0);
        }
    }
}
