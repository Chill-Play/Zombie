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
    public event System.Action onAdsShowed;
    [SerializeField] ColliderEventListener showButtonsCollider;
    public ResourceBox[] ResourcesBoxes => resourcesBoxes;
    public event System.Action OnShopOpened;
    public event System.Action OnShopClosed;
    public event System.Action<int, ResourceType, int, int> OnUpdateSellButton;
    public event System.Action<int, ResourceType, int> OnUpdateGetResourceButton;

    [SerializeField] private bool ohShitHereWeGoAgain;
    
    private void Awake()
    {
        Debug.Log("Ads count: " + adsCount);
        resources = ResourcesController.Instance.OpenedResources;
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
        if (obj.TryGetComponent<UnitInteracting>(out var unitInteracting))
        {
            foreach (var resourceBox in resourcesBoxes)
                resourceBox.SetUserTransform(unitInteracting.transform);
            OnShopOpened?.Invoke();
        }
    }

    void Shop_OnTriggerExitEvent(Collider obj)
    {
        if (obj.TryGetComponent<UnitInteracting>(out var unitInteracting))
            OnShopClosed?.Invoke();
    }

    public void UpdateShop()
    {
        int tmp = 1;
        int i = 1;
        if (ohShitHereWeGoAgain)
        {
            for (int j = 0; j < 2; j++)
            {
                while (i == tmp && resources.Count - 1 > 1)
                    i = Random.Range(1, resources.Count);
                tmp = i;
                var playerResources = ResourcesController.Instance.ResourcesCount;
                int count = Mathf.Max(minCount, playerResources.Count(resources[i]) * sellPercent / 100);
                int price = count * resources[i].price;
                OnUpdateSellButton?.Invoke(j, resources[i], count, price);
            }

            tmp = 1;
            i = 1;
        }
        for (int j = 0; j < 2; j++)
        {
            while (i == tmp && resources.Count - 1> 1)
                i = Random.Range(1, resources.Count);
            tmp = i;
            resourcesBoxes[j].ShowResource(resources[i]);
            ResourceType resourceType = resources[i];
            int count = ADScoefficient * (resourceType.price + adsCount);
            OnUpdateGetResourceButton?.Invoke(j, resourceType, count);
        }
    }
}