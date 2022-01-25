using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FreeResourcesButtonUI : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text countText;

    public void Show(ResourceType resourceType, int count, System.Action<ResourceType, int> callback)
    {
        transform.DOScale(new Vector2(1f, 1f), .3f).SetEase(Ease.OutBack);
        gameObject.SetActive(true);
        button.onClick.AddListener(() =>
        {
            button.onClick.RemoveAllListeners();
            callback?.Invoke(resourceType, count);
        });
        icon.sprite = resourceType.icon;
        countText.text = "+" + count.ToString();
    }

    public void Hide()
    {
        button.onClick.RemoveAllListeners();
        transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InBack);
    }
  
}
