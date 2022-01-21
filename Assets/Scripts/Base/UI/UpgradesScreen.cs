using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesScreen : UIScreen, IShowScreen
{
    const int MINIMAL_STAT_LEVEL_TO_FREE_OPTION = 2;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text label;
    [SerializeField] List<StatUpgradeCard> cards;
    [SerializeField] Transform panel;

    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;
    InputPanel inputPanel;
    private Tween scaleTween;

    private void Awake()
    {
        inputPanel = FindObjectOfType<InputPanel>();
    }

    public void Show(UpgradeZone zone, string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, System.Action onClose = null)
    {
        inputPanel.DisableInput();
        this.onClose = onClose;
        label.text = name;
        this.stats = stats;
        this.availableResources = availableResources;
        if(cards.Count != stats.Count)
        {
            Debug.LogError("Wrong setup for upgrade screen");
            return;
        }
        UpdateButtons(zone);
        ShowAnimation();
    }
    

    void UpdateButtons(UpgradeZone zone)
    {
        int freeSlot = GetFreeSlotId(zone);

        for (int i = 0; i < cards.Count; i++)
        {
            var type = stats[i].Item1;
            var info = stats[i].Item2;
            int tmp = i;
            cards[i].Setup(info, type, availableResources, i == freeSlot, (free) =>
            {
                /*if (free)
                {
                    AdvertisementManager.Instance.ShowRewardedVideo((result) =>
                    {
                        zone.FreeUpgradeAvailable = false;
                        if (result) UpgradeStat(zone, info, type, free);
                    }, "base_shop_free_stat");
                }
                else
                {
                    UpgradeStat(zone, info, type, free);
                }*/
                
                UpgradeStat(zone, info, type, free);
                
                cards[tmp].transform.localScale = Vector3.one;
                if (scaleTween != null)
                    scaleTween.Kill(true);
                scaleTween = cards[tmp].transform.DOPunchScale(new Vector2(.1f, .1f), .3f, 7, 1).OnComplete(() =>
                {
                    cards[tmp].transform.localScale = Vector3.one;
                });
            });
        }
    }

    int GetFreeSlotId(UpgradeZone zone)
    {

        bool upgradesAvailable = false;
        bool equallevel = true;
        int lowestlevelSlot = -1;
        int lowestlevel = int.MaxValue;
        int maxlevel = -1;

        for (int i = 0; i < cards.Count; i++)
        {
            var type = stats[i].Item1;
            var info = stats[i].Item2;

            upgradesAvailable = upgradesAvailable || type.GetLevelCost(info.level).IsFilled(availableResources);

            if (lowestlevel != info.level && lowestlevel != int.MaxValue)
            {
                equallevel = false;
            }
            if (lowestlevel > info.level)
            {
                lowestlevel = info.level;
                lowestlevelSlot = i;
            }

            if (maxlevel < info.level)
            {
                maxlevel = info.level;
            }
        }

        if (!zone.FreeUpgradeAvailable || upgradesAvailable || maxlevel < MINIMAL_STAT_LEVEL_TO_FREE_OPTION) //|| !AdvertisementManager.Instance.RewardedAvailable
        {
            return -1;
        }
        else
        {
            if (equallevel)
            {
                return Random.Range(0, cards.Count);
            }
            else
            {
                return lowestlevelSlot;
            }
        }       
    }

    void UpgradeStat(UpgradeZone zone, StatInfo info, StatsType type, bool free)
    {
        if (!free)
        {
            var cost = type.GetLevelCost(info.level);
            availableResources.Subtract(cost);
            FindObjectOfType<ResourcesController>().UpdateResources();
        }
        
        FindObjectOfType<StatsManager>().AddStatLevel(type, free);        
        UpdateButtons(zone);
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
        inputPanel.EnableInput();
        var sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InCirc));
        sequence.Join(DOTween.To(() => canvasGroup.alpha, (x) => canvasGroup.alpha = x, 0f, 0.25f).SetEase(Ease.InCirc)); 
        sequence.OnComplete(() =>
        {
            onClose?.Invoke();
        });
    }
}
