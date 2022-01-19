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
    

    public void Setup(ResourceType resourceType, int count, System.Action click)
    {
        onAdsShowed += click;
        resource.sprite = resourceType.icon;
        countText.text = count.ToString();
    }

    public void ShowADS()
    {
        //show add
        //maybe add ads callback
        //if ads 
        onAdsShowed?.Invoke();
        Hide();
    }
}