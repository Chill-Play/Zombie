using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesScreen : UIScreen
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text label;
    [SerializeField] List<StatUpgradeCard> cards;
    [SerializeField] Transform panel;

    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;
    
    public void Show(string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, System.Action onClose = null)
    {
        this.onClose = onClose;
        label.text = name;
        this.stats = stats;
        this.availableResources = availableResources;
        if(cards.Count != stats.Count)
        {
            Debug.LogError("Wrong setup for upgrade screen");
            return;
        }
        UpdateButtons();
        ShowAnimation();
    }
    

    void UpdateButtons()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var type = stats[i].Item1;
            var info = stats[i].Item2;
            cards[i].Setup(info, type, availableResources, () =>
            {
                var cost = type.GetLevelCost(info.level);
                availableResources.Subtract(cost);
                FindObjectOfType<StatsManager>().AddStatLevel(type);
                FindObjectOfType<ResourcesController>().UpdateResources();
                UpdateButtons();
            });
        }
    }


    void ShowAnimation()
    {
        foreach(var card in cards)
        {
            card.transform.localScale = Vector3.zero;
        }
        canvasGroup.alpha = 0f;
        panel.transform.localScale = Vector3.one * 0.3f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic, 1.1f, 0.4f));
        sequence.Join(DOTween.To(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, 1f, 0.25f).SetEase(Ease.OutSine));
        foreach (var card in cards)
        {
            sequence.Append(card.transform.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.1f, 0.2f));// = Vector3.zero;
        }

    }


    public void Close()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InCirc));
        sequence.Join(DOTween.To(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, 0f, 0.25f).SetEase(Ease.InCirc)); 
        sequence.OnComplete(() =>
        {
            onClose?.Invoke();
        });
    }
}
