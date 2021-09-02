using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialText : MonoBehaviour
{
    [SerializeField] ConditionTrigger showConditionTrigger;
    [SerializeField] ConditionTrigger hideTrigger;

    [SerializeField] float timeBeforeShowing = 0f;
    [SerializeField] float timeToHide = 0.3f;

    Vector3 scale;

    private void Awake()
    {
        scale = transform.localScale;

        if (showConditionTrigger != null)
        {
            gameObject.SetActive(false);
            showConditionTrigger.OnTrigger += ShowConditionTrigger_OnTrigger;
        }

        if (hideTrigger != null)
        {
            hideTrigger.OnTrigger += HideTrigger_OnTrigger; ;
        }
    }

    private void HideTrigger_OnTrigger()
    {
        transform.DOKill(true); 
        transform.DOScale(Vector3.zero, timeToHide).SetEase(Ease.InCirc).OnComplete(() => gameObject.SetActive(false));
    }

    private void ShowConditionTrigger_OnTrigger()
    {
        gameObject.SetActive(true);
        transform.DOKill(true);        
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(timeBeforeShowing);
        sequence.Append(transform.DOScale(scale, 0.3f).SetEase(Ease.OutCirc));
    }
}
