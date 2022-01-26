using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UpgradesSpecialistsScreen : UIScreen, IShowScreen
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text label;
    [SerializeField] Transform panel;
    [SerializeField] private Transform cardsSpawnPoint;
    [SerializeField] private SpecialistUpgradeCardUI specialitsUpgradeCardPrefab;
    [SerializeField] FreeResourcesButtonUI freeResourcesButton;

    private CardController cardController;
    private List<SpecialistUpgradeCardUI> cards = new List<SpecialistUpgradeCardUI>();
    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;
    ResourcesController resourcesController;
    InputPanel inputPanel;
    RewardController rewardController;

    private CardsInfo activeCards => cardController.ActiveCards;


    private void Awake()
    {
        inputPanel = InputPanel.Instance;
        cardController = CardController.Instance;
        resourcesController = ResourcesController.Instance;
        rewardController = RewardController.Instance;
    }

    public void Show(UpgradeZone zone, string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, Action onClose = null)
    {
        inputPanel.DisableInput();
        this.onClose = onClose;
        label.text = name;
        this.stats = stats;
        this.availableResources = availableResources;
        ShowCards();
        UpdateCards(stats[0].Item1);
    }

    void ShowCards()
    {
        var seq = DOTween.Sequence();
        panel.localScale = Vector3.zero;
        int i = 0;
    
        seq.Append(panel.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.3f, .7f));

        for (; i < activeCards.Count; i++)
        {
            if (i < cards.Count)
                cards[i].gameObject.SetActive(true);
            else
                cards.Add(Instantiate(specialitsUpgradeCardPrefab, cardsSpawnPoint));      
            
            SpecialistUpgradeCardUI upgradeCard = cards[i];
            upgradeCard.transform.localScale = Vector3.zero;       

            seq.AppendInterval(i * .1f);
            seq.AppendCallback(() =>
            {
                upgradeCard.transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.3f, .7f);
            });
        }

        for (; i < cards.Count; i++)
        {
            cards[i].gameObject.SetActive(false);
        } 
    }

    void UpdateCards(StatsType statType) //refactor pls
    {
        bool freeResourcesOption = false;

        for (int i = 0; i < activeCards.Count; i++)
        {
             Card card = activeCards.cardSlots[i].card;
             SpecialistUpgradeCardUI upgradeCard = cards[i];
             var cardStats = cardController.CardStats(card);
             int lvl = cardStats.statsInfo[statType];
             var cost = statType.GetLevelCost(lvl);

            if (!freeResourcesOption && cost.TryGetMissingResource(availableResources, out var missingResourceType))
            {
                freeResourcesButton.Show(missingResourceType, rewardController.GetResourcesRewardCount(missingResourceType), (x, y) => AddFreeResourcesClicked(statType, x, y));
                freeResourcesOption = true;
            }    

            upgradeCard.Setup(card, statType, resourcesController.ResourcesCount, () => UpgradeCardStat(card, statType));
        }

        if (!freeResourcesOption)
        {
            freeResourcesButton.Hide();
        }
    }

    void AddFreeResourcesClicked(StatsType statType, ResourceType resourceType, int count)
    {
        rewardController.AddResourceRewardLevel(resourceType);
        resourcesController.AddResources(resourceType, count);
        resourcesController.UpdateResources();
        UpdateCards(statType);
    }

    void UpgradeCardStat(Card card, StatsType statType) //refactor pls
    {
        cardController.UpgradeCardStats(card, statType);
        UpdateCards(statType);
    }

    public void Hide()
    {
        inputPanel.EnableInput();
        var seq = DOTween.Sequence();
        for (int i = activeCards.Count - 1; i >= 0; i--)
        {
            SpecialistUpgradeCardUI upgradeCard = cards[i];
            seq.AppendInterval((activeCards.Count - i -1) * .1f);
            seq.AppendCallback(() =>
            {
                upgradeCard.transform.DOScale(Vector3.zero, .4f).SetEase(Ease.InElastic, 1.3f, .7f).OnComplete(() =>
                {
                    upgradeCard.gameObject.SetActive(false);
                });
                // upgradeCard.transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.3f, .7f);
            });
        }

        seq.AppendInterval(activeCards.Count * .1f);
        seq.Append(panel.DOScale(Vector3.zero, .4f).SetEase(Ease.InElastic,1.3f, .7f).OnComplete(() =>
        {
            onClose?.Invoke();
        }));
    }
}
