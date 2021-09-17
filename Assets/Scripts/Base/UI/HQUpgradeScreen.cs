using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HQUpgradeScreen : UIScreen
{
    const int MINIMAL_HQ_LEVEL_TO_FREE_OPTION = 2;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] HQUpgradeCard card;
    [SerializeField] TMP_Text levelLabel;
    [SerializeField] Transform panel;
    System.Action onClose;
    ResourcesInfo availableResources;


    public void Show(ResourcesInfo availableResources, System.Action onClose)
    {
        this.onClose = onClose;
        this.availableResources = availableResources;
        UpdateButton();
        ShowAnimation();
    }


    void UpdateButton()
    {
        HQBuilding hq = FindObjectOfType<HQBuilding>();
        var cost = hq.GetCostForLevelUp();
        var level = hq.Level;
        levelLabel.text = "LVL " + (level + 1);
        bool freeOption = (level + 1) >= MINIMAL_HQ_LEVEL_TO_FREE_OPTION;
        card.Setup(level + 2, cost, availableResources, freeOption, (free) =>
        {
            if (free)
            {
                AdvertisementManager.Instance.ShowRewardedVideo((result) =>
                {
                    if (result) LevelUp(hq, cost, free);
                });
            }
            else
            {
                LevelUp(hq, cost, free);
            }
        });
    }

    void LevelUp(HQBuilding hq, ResourcesInfo cost, bool free)
    {
        if (!free)
        {
            availableResources.Subtract(cost);
            FindObjectOfType<ResourcesController>().UpdateResources();
        }
        hq.LevelUp();
        UpdateButton();
    }


    void ShowAnimation()
    {
        card.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        panel.transform.localScale = Vector3.one * 0.3f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic, 1.1f, 0.4f));
        sequence.Join(DOTween.To(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, 1f, 0.25f).SetEase(Ease.OutSine));
        sequence.Append(card.transform.DOScale(1f, 0.3f).SetEase(Ease.OutElastic, 1.1f, 0.2f));

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
