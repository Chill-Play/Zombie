using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialText : MonoBehaviour
{
    [SerializeField] ConditionTrigger showConditionTrigger;
    [SerializeField] ConditionTrigger hideTrigger;
    [SerializeField] Transform content;


    [SerializeField] float timeBeforeShowing = 0f;
    [SerializeField] float timeToHide = 0.3f;

    Vector3 scale;

    private void Start()
    {
        if (content == null)
        {
            content = transform;
        }

        scale = content.localScale;

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
        content.DOKill(true);
        content.DOScale(Vector3.zero, timeToHide).SetEase(Ease.InCirc).OnComplete(() => gameObject.SetActive(false));
    }

    private void ShowConditionTrigger_OnTrigger()
    {    
        gameObject.SetActive(true);
        content.DOKill(true);
        content.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(timeBeforeShowing);
        sequence.Append(content.DOScale(scale, 0.3f).SetEase(Ease.OutCirc));
    }
}
