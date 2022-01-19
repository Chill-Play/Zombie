using UnityEngine;
using DG.Tweening;

public class ShopButton : MonoBehaviour
{ 
    public void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one,.1f).SetEase(Ease.InElastic, 1.1f, .3f);
    }

    public void Hide()
    {
        transform.DOScale(Vector3.zero,.1f).SetEase(Ease.InElastic, 1.1f, .3f).OnComplete(()=>{gameObject.SetActive(false);});
    }
}