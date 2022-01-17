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
        seq.Append(panel.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutBack));
        seq.AppendInterval(.4f);
        for (; i < activeCards.Count; i++)
        {
            if (i < cards.Count)
                cards[i].gameObject.SetActive(true);
            else
                cards.Add(Instantiate(specialitsUpgradeCardPrefab, cardsSpawnPoint));
            Card card = activeCards.cardSlots[i].card;
            cards[i].transform.localScale = Vector3.zero;
            cards[i].Setup(card, statType,resourcesController.ResourcesCount, () => UpgradeCardStat(card, statType));
            seq.Join(cards[i].gameObject.transform.DOScale(new Vector3(1, 1, 1), .4f + i * .4f).SetEase(Ease.OutBack));
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
        var seq = DOTween.Sequence();
        for (int i = activeCards.Count - 1; i >= 0; i--)
        {
            seq.Join(cards[i].transform.DOScale(Vector3.zero, .3f + (activeCards.Count - i - 1) * .3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                cards[i].gameObject.SetActive(false);
            }));
        }
        seq.Append(panel.DOScale(Vector3.zero, .3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            onClose?.Invoke();
        }));
    }
}
