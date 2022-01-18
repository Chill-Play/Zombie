using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GetResourceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image resource;
    private System.Action onAdsShowed;

    private void Show()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one,.1f).SetEase(Ease.InElastic, 1.1f, .3f);
    }

    private void Hide()
    {
        transform.DOScale(Vector3.zero,.1f).SetEase(Ease.InElastic, 1.1f, .3f).OnComplete(() => { gameObject.SetActive(false); });
    }

    public void Setup(ResourceType resourceType, int count, System.Action click)
    {
        onAdsShowed += click;
        resource.sprite = resourceType.icon;
        countText.text = count.ToString();
        Show();
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