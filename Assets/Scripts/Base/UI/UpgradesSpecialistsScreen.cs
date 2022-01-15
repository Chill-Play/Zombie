using System;
using System.Collections.Generic;
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

        int i = 0;
        for (; i < activeCards.Count; i++)
        {
            if (i < cards.Count)
                cards[i].gameObject.SetActive(true);
            else
                cards.Add(Instantiate(specialitsUpgradeCardPrefab, cardsSpawnPoint));
            Card card = activeCards.cardSlots[i].card;
            cards[i].Setup(card, statType,resourcesController.ResourcesCount, () => UpgradeCardStat(card, statType));
        }

        for (; i < cards.Count; i++)
            cards[i].gameObject.SetActive(false);
    }

    void UpgradeCardStat(Card card, StatsType statType)
    {
        cardController.UpgradeCardStats(card, statType);
        UpdateCards(statType);
    }

    public void Hide()
    {
        onClose?.Invoke();
    }
}
