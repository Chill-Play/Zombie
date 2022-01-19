using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class SellResourceButton : ShopButton
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image money;
    [SerializeField] private Image resource;
    private System.Action onBuy;

    public void Setup(ResourceType resourceType, int count,int price, System.Action click)
    {
        onBuy += click;
        priceText.text = price.ToString();
        money.sprite = ResourcesController.Instance.OpenedResources[0].icon;
        resource.sprite = resourceType.icon;
        countText.text = count.ToString();
    }

    public void Buy()
    {
        onBuy?.Invoke();
    }
}