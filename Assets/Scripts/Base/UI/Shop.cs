using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Shop : BaseObject
{
    [BaseSerialize] protected int adsCount;
    [SerializeField] public Transform[] buttonPos;
    [SerializeField] private int sellPercent;
    [SerializeField] private int ADScoefficient;
    [SerializeField] private int minCount;
    [SerializeField] private ResourceBox[] resourcesBoxes;
    private List<ResourceType> resources = new List<ResourceType>();
    private UIShopScreenView uiShopScreenView;
    public event System.Action onAdsShowed;
    [SerializeField] ColliderEventListener showButtonsCollider;
    
    private void Awake()
    {
        Debug.Log("Ads count: " + adsCount);
        resources = ResourcesController.Instance.OpenedResources;
        uiShopScreenView = FindObjectOfType<UIShopScreenView>();
        showButtonsCollider.OnTriggerEnterEvent += Shop_OnTriggerEnterEvent;
        showButtonsCollider.OnTriggerExitEvent += Shop_OnTriggerExitEvent;
        UpdateShop();
    }

    public void IncerementAdsCount()
    {
        adsCount++;
        RequireSave();
    }
    void Shop_OnTriggerEnterEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
            uiShopScreenView.ShowButtons();
    }

    void Shop_OnTriggerExitEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
            uiShopScreenView.HideButtons();
    }

    public void UpdateShop()
    {
        int tmp = 1;
        int i = 1;
        for (int j = 0; j < 2; j++)
        {
            while (i == tmp && resources.Count - 1 > 1)
                i = Random.Range(1, resources.Count);
            tmp = i;
            var playerResources = ResourcesController.Instance.ResourcesCount;
            int count = Mathf.Max(minCount, playerResources.Count(resources[i]) * sellPercent/ 100);
            int price = count * resources[i].price;
            uiShopScreenView.UpdateSellButton(j, resources[i], count, price);
        }
        tmp = 1;
        i = 1;
        for (int j = 0; j < 2; j++)
        {
            while (i == tmp && resources.Count > 1)
                i = Random.Range(0, resources.Count);
            tmp = i;
            resourcesBoxes[j].ShowResource(resources[i]);
            ResourceType resourceType = resources[i];
            int count = ADScoefficient * (resourceType.price + adsCount);
            uiShopScreenView.UpdateGetResourceButton(j, resourceType, count);
        }
    }
}