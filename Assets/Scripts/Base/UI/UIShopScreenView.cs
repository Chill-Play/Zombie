using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIShopScreenView : MonoBehaviour
{
    [SerializeField] private Transform shop;
    [SerializeField] private Transform buttons;
    [SerializeField] private Vector3 alignment;
    [SerializeField] private GetResourceButton[] getResourceButtons;
    [SerializeField] private SellResourceButton[] sellResourceButtons;
    [SerializeField] private int sellPercent;
    [SerializeField] private int ADSPercent;
    [SerializeField] private int minSellCount;
    private List<ResourceType> resources = new List<ResourceType>();
    private void Awake()
    {
        resources = FindObjectOfType<ResourcesController>().OpenedResources;
        UpdateSellButtons();
        UpdateGetResourceButtons();
    }

    void UpdateSellButtons()
    {
        foreach (var button in sellResourceButtons)
        {
            ResourceType resourceType = resources[Random.Range(1, resources.Count)];
            var playerResources = ResourcesController.Instance.ResourcesCount;
            int count = Mathf.Max(minSellCount, playerResources.Count(resourceType) * sellPercent/ 100);
            //int count =playerResources.Count(resourceType) * sellPercent / 100;
            int price = count * resourceType.price;
            button.Setup(resourceType, count, price, () =>
            {
                if (playerResources.Count(resourceType) >= count)
                {
                    playerResources.Subtract(resourceType, count);
                    playerResources.Add(resources[0], price);
                    ResourcesController.Instance.UpdateResources();
                }
            });
        }
    }

    void UpdateGetResourceButtons()
    {
        foreach (var button in getResourceButtons)
        {
            ResourceType resourceType = resources[Random.Range(0, resources.Count)];
            var playerResources = ResourcesController.Instance.ResourcesCount;
            int count = Mathf.Max(minSellCount, playerResources.Count(resourceType) * ADSPercent/ 100);
            button.Setup(resourceType, count, () =>
            {
                playerResources.Add(resourceType, count);
                ResourcesController.Instance.UpdateResources();
            });
        }
    }
    
    void LateUpdate()
    {
        if (shop)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(shop.transform.position);
            screenPos.z = 0f;
            buttons.position = screenPos + alignment;
        }
    }
}