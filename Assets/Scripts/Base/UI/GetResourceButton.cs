using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GetResourceButton : ShopButton
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image resource;
    private System.Action onAdsShowed;
    public bool adsShowed;
    

    public void Setup(ResourceType resourceType, int count, System.Action click)
    {
        transform.localScale = Vector3.zero;
        onAdsShowed += click;
        resource.sprite = resourceType.icon;
        countText.text = count.ToString();
    }

    public void ShowADS()
    {
        //maybe add ads callback
        //if ads 
        onAdsShowed?.Invoke();
        transform.DOScale(Vector3.zero,.1f).SetEase(Ease.InElastic, 1.1f, .3f).OnComplete(() =>
        {
            adsShowed = true;
            gameObject.SetActive(false);
        });
    }
}