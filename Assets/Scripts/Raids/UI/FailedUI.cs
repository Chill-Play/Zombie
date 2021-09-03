using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailedUI : UIScreen
{
    [SerializeField] Image background;
    [SerializeField] Transform upperPanel;
    [SerializeField] Transform middlePanel;
    [SerializeField] Transform toBaseButton;

    void Start()
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        upperPanel.transform.localScale = Vector3.zero;
        toBaseButton.transform.localScale = Vector3.zero;
        middlePanel.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(background.DOFade(0.6f, 0.2f));
        sequence.Append(upperPanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        sequence.Append(middlePanel.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
        sequence.AppendInterval(0.1f);
        sequence.Append(toBaseButton.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.1f, 0.3f));
    }


    public void ToBase()
    {
        LevelController.Instance.ToBase(false);
    }
}
