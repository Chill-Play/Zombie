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
    [SerializeField] TextMeshProUGUI failText;
    [SerializeField] private Image money;
    [SerializeField] private Image resource;
    private System.Action onBuy;
    private Vector2 startFailTextPos;
    private Sequence seq;
    
    public void Setup(ResourceType resourceType, int count,int price, System.Action click)
    {
        transform.localScale = Vector3.zero;
        startFailTextPos = failText.rectTransform.anchoredPosition;
        onBuy += click;
        priceText.text = price.ToString();
        money.sprite = ResourcesController.Instance.OpenedResources[0].icon;
        resource.sprite = resourceType.icon;
        countText.text = count.ToString();
    }

    public void BuyFailed()
    {
        seq.Complete();
        seq.Kill();
        failText.gameObject.SetActive(true);
        failText.color = new Color(failText.color.r,failText.color.g,failText.color.b,0);
        var textTransform = failText.rectTransform;
        seq = DOTween.Sequence();
        seq.Append(DOTween.ToAlpha(()=> failText.color, x=> failText.color = x, 1, 1f));
        seq.Join(DOTween.ToAxis(() => textTransform.anchoredPosition, y => textTransform.anchoredPosition = y, 140, 1, AxisConstraint.Y));
        seq.Append(DOTween.ToAlpha(()=> failText.color, x=> failText.color = x, 0, 1f));
        seq.Join(DOTween.ToAxis(()=> textTransform.anchoredPosition, y=> textTransform.anchoredPosition = y, 180, 1,AxisConstraint.Y).OnComplete(() =>
        {
            failText.rectTransform.anchoredPosition = startFailTextPos;
            failText.gameObject.SetActive(false);
        }));
                
    }

    public void Buy()
    {
        onBuy?.Invoke();
    }
}