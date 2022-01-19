using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIShopScreenView : MonoBehaviour
{
    [SerializeField] private GetResourceButton[] getResourceButtons;
    [SerializeField] private SellResourceButton[] sellResourceButtons;
    private List<ResourceType> resources = new List<ResourceType>();
    private Shop shop;
    private void Awake()
    {
        resources = ResourcesController.Instance.OpenedResources;
        shop = FindObjectOfType<Shop>();
    }

    public void ShowButtons()
    {
        foreach (var button in getResourceButtons)
            if (!button.adsShowed)
                button.Show();
        foreach (var button in sellResourceButtons)
            button.Show();
    }

    public void HideButtons()
    {
        foreach (var button in getResourceButtons)
            button.Hide();
        foreach (var button in sellResourceButtons)
            button.Hide();
    }

    public void UpdateSellButton(int index, ResourceType resourceType, int count, int price)
    {
        Debug.Log("sell button index: " + index);
        var button = sellResourceButtons[index];
        var playerResources = ResourcesController.Instance.ResourcesCount;
        button.Setup(resourceType, count, price, () =>
        {
            if (playerResources.Count(resourceType) >= count)
            {
                playerResources.Subtract(resourceType, count);
                playerResources.Add(resources[0], price);
                ResourcesController.Instance.UpdateResources();
            }
            else
            {
                button.BuyFailed();
            }
        });
    }

    public void UpdateSellButtons(int minCount, int sellPercent)
    {
        int tmp = 1;
        int i = 1;
        foreach (var button in sellResourceButtons)
        {
            while (i == tmp && resources.Count - 1 > 1)
                i = Random.Range(1, resources.Count);
            tmp = i;
            ResourceType resourceType = resources[i];
            var playerResources = ResourcesController.Instance.ResourcesCount;
            int count = Mathf.Max(minCount, playerResources.Count(resourceType) * sellPercent/ 100);
            int price = count * resourceType.price;
            button.Setup(resourceType, count, price, () =>
            {
                if (playerResources.Count(resourceType) >= count)
                {
                    playerResources.Subtract(resourceType, count);
                    playerResources.Add(resources[0], price);
                    ResourcesController.Instance.UpdateResources();
                }
                else
                {
                    button.BuyFailed();
                }
            });
        }
    }

    public void UpdateGetResourceButton(int index, ResourceType resourceType, int count)
    {
        Debug.Log("get button index: " + index);
        var playerResources = ResourcesController.Instance.ResourcesCount;
        var button = getResourceButtons[index];
        button.Setup(resourceType, count, () =>
        {
            playerResources.Add(resourceType, count);
            shop.IncerementAdsCount();
            ResourcesController.Instance.UpdateResources();
        });
    }
    public void UpdateGetResourceButtons(int ADScoefficient, int adsCount)
    {
        int tmp = 1;
        int i = 1;
        foreach (var button in getResourceButtons)
        {
            while (i == tmp && resources.Count > 1)
                i = Random.Range(0, resources.Count);
            tmp = i;
            ResourceType resourceType = resources[i];
            var playerResources = ResourcesController.Instance.ResourcesCount;
            int count = ADScoefficient * (resourceType.price + adsCount);
            button.Setup(resourceType, count, () =>
            {
                playerResources.Add(resourceType, count);
                shop.IncerementAdsCount();
                ResourcesController.Instance.UpdateResources();
            });
        }
    }

    void SetButtonsPosition()
    {
        int i = 0;
        for (; i < shop.buttonPos.Length/2; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(shop.buttonPos[i].transform.position);
            screenPos.z = 0f;
            getResourceButtons[i].transform.position = screenPos;
        }

        for (; i < shop.buttonPos.Length; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(shop.buttonPos[i].transform.position);
            screenPos.z = 0f;
            sellResourceButtons[i - shop.buttonPos.Length/2].transform.position = screenPos;
        }
    }
    
    void LateUpdate()
    {
        SetButtonsPosition();
    }
}