using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class SellResourceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image money;
    [SerializeField] private Image resource;
    private System.Action onBuy;

    private void Show()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one,.1f).SetEase(Ease.InElastic, 1.1f, .3f);
    }

    private void Hide()
    {
        transform.DOScale(Vector3.zero,.1f).SetEase(Ease.InElastic, 1.1f, .3f).OnComplete(() => { gameObject.SetActive(false); });
    }

    public void Setup(ResourceType resourceType, int count,int price, System.Action click)
    {
        onBuy += click;
        priceText.text = price.ToString();
        money.sprite = ResourcesController.Instance.OpenedResources[0].icon;
        resource.sprite = resourceType.icon;
        countText.text = count.ToString();
        Show();
    }

    public void Buy()
    {
        onBuy?.Invoke();
    }
}