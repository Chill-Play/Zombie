using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ZombieCountUI : MonoBehaviour
{
    [SerializeField] TMP_Text countText;

    Tween updateTween;

    public void SetupCount(int count)
    {
        countText.text = count.ToString(); 
    }

    public void UpdateCount(int count)
    {
        updateTween.Kill(true);
        countText.text = count.ToString();
        updateTween = countText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1f).SetEase(Ease.InCirc);
    }


}
