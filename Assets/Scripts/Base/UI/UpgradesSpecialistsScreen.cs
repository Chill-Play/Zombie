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

    private CardController cardController;
    private List<SpecialistUpgradeCardUI> cards = new List<SpecialistUpgradeCardUI>();
    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;
    ResourcesController resourcesController;
    private CardsInfo activeCards => cardController.ActiveCards;


    private void Awake()
    {
        cardController = FindObjectOfType<CardController>();
        resourcesController = FindObjectOfType<ResourcesController>();
    }

    public void Show(UpgradeZone zone, string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, Action onClose = null)
    {
        this.onClose = onClose;
        label.text = name;
        this.stats = stats;
        this.availableResources = availableResources;
        UpdateCards(stats[0].Item1);
    }

    void UpdateCards(StatsType statType)
    {
        var seq = DOTween.Sequence();
        panel.localScale = Vector3.zero;
        int i = 0;
        // seq.AppendInterval(10f);
        seq.Append(panel.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.3f, .7f));
        for (; i < activeCards.Count; i++)
        {
            if (i < cards.Count)
                cards[i].gameObject.SetActive(true);
            else
                cards.Add(Instantiate(specialitsUpgradeCardPrefab, cardsSpawnPoint));
            Card card = activeCards.cardSlots[i].card;
            SpecialistUpgradeCardUI upgradeCard = cards[i];
            upgradeCard.transform.localScale = Vector3.zero;
            upgradeCard.Setup(card, statType,resourcesController.ResourcesCount, () => UpgradeCardStat(card, statType));
            
            seq.AppendInterval(i * .1f);
            seq.AppendCallback(() =>
            {
                upgradeCard.transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutElastic, 1.3f, .7f);
            });
        }
        for (; i < cards.Count; i++)
            cards[i].gameObject.SetActive(false);
    }

    void UpgradeCardStat(Card card, StatsType statType)
    {
        cardController.UpgradeCardStats(card, statType);
        for(int i = 0; i < activeCards.Count; i++) 
            cards[i].Setup(card, statType,resourcesController.ResourcesCount, () => UpgradeCardStat(card, statType));
        //UpdateCards(statType);
    }

    public void Hide()
    {
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
