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
    [SerializeField] private bool ohShitHereWeGoAgain;
    private void Awake()
    {
        resources = ResourcesController.Instance.OpenedResources;
        shop = FindObjectOfType<Shop>();
        shop.OnShopOpened += ShowButtons;
        shop.OnShopClosed += HideButtons;
        if(ohShitHereWeGoAgain)
        {
            shop.OnUpdateSellButton += UpdateSellButton;
        }
        shop.OnUpdateGetResourceButton += UpdateGetResourceButton;
    }

    public void ShowButtons()
    {
        foreach (var button in getResourceButtons)
            if (!button.adsShowed)
                button.Show();
        if (ohShitHereWeGoAgain)
        {
            foreach (var button in sellResourceButtons)
                button.Show();
        }
    }

    public void HideButtons()
    {
        foreach (var button in getResourceButtons)
            button.Hide();
        if (ohShitHereWeGoAgain)
        {
            foreach (var button in sellResourceButtons)
                button.Hide();
        }
    }

    public void UpdateSellButton(int index, ResourceType resourceType, int count, int price)
    {
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

    public void UpdateGetResourceButton(int index, ResourceType resourceType, int count)
    {
        var playerResources = ResourcesController.Instance.ResourcesCount;
        var button = getResourceButtons[index];
        button.Setup(resourceType, count, () =>
        {
            shop.ResourcesBoxes[index].SpawnResources(resourceType, count);
            //playerResources.Add(resourceType, count);
            shop.IncerementAdsCount();
            ResourcesController.Instance.UpdateResources();
        });
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
        if (ohShitHereWeGoAgain)
        {
            for (; i < shop.buttonPos.Length; i++)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(shop.buttonPos[i].transform.position);
                screenPos.z = 0f;
                sellResourceButtons[i - shop.buttonPos.Length / 2].transform.position = screenPos;
            }
        }
    }
    
    void LateUpdate()
    {
        SetButtonsPosition();
    }
}