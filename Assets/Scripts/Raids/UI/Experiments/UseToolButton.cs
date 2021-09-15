using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UseToolButton : MonoBehaviour
{
    public event System.Action<ResourceType> OnToolUsed;

    [SerializeField] Image notActiveImage;
    [SerializeField] Button button;

    ResourceType resourceTool;

    public void ShowButton(ResourceType resourceTool, bool isActive)
    {
        this.resourceTool = resourceTool;       
        gameObject.SetActive(true);
        button.interactable = isActive;
        notActiveImage.gameObject.SetActive(!isActive);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCirc);       
    }

    public void HideButton()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => gameObject.SetActive(false));        
    }    

    public void OnButtonDown()
    {
        OnToolUsed?.Invoke(resourceTool);
    }
}
