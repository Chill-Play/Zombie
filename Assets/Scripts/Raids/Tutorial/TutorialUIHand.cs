using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialUIHand : MonoBehaviour
{
    [SerializeField] float timeToShow = 2f;
    [SerializeField] ConditionTrigger disableTrigger;
    [SerializeField] Transform content;

    Squad squad;
    float notMovingTime;

    Vector3 scale;


    private void Awake()
    {
        scale = transform.localScale;
        squad = FindObjectOfType<Squad>();
        disableTrigger.OnTrigger += DisableTrigger_OnTrigger;
        content.gameObject.SetActive(false);
    }

    private void DisableTrigger_OnTrigger()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!squad.IsMoving)
        {
            if (!content.gameObject.activeSelf)
            {
                notMovingTime += Time.deltaTime;
                if (notMovingTime >= timeToShow)
                {
                    Show();
                }
            }
        }
        else
        {
            notMovingTime = 0f;
            if (content.gameObject.activeSelf)
            {                               
                Hide();
            }
        }
       
    }

    void Show()
    {
        content.DOKill(true);
        content.gameObject.SetActive(true);
        content.localScale = Vector3.zero;
        content.DOScale(scale, 0.3f).SetEase(Ease.OutCirc);
    }

    void Hide()
    {
        content.DOKill(true);
        content.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCirc).OnComplete(() => content.gameObject.SetActive(false));
    }

}
