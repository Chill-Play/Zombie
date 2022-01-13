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
    private CardController cardController;
    [SerializeField] private GameObject cardPrefab;
    private List<GameObject> cards = new List<GameObject>();
    [SerializeField] Transform panel;
    [SerializeField] private Transform cardsSpawnPoint;

    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;
    private CardsInfo activeCards => cardController.ActiveCards;
    
    private void Awake()
    {
        cardController = FindObjectOfType<CardController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            UpdateCards();
    }

    public void Show(UpgradeZone zone, string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, Action onClose = null)
    {
        this.onClose = onClose;
        label.text = name;
        this.stats = stats;
        this.availableResources = availableResources;
        UpdateCards();
    }

    void UpdateCards()
    {
        int i = 0;
        for (; i < activeCards.Count; i++)
        {
            if (i < cards.Count)
                cards[i].SetActive(true);
            else
                cards.Add(Instantiate(cardPrefab, cardsSpawnPoint));
            CardSlot cardSlot = activeCards.cardSlots[i];
            // CardStatsSlot stats = cardController.CardStats(activeCards.cardSlots[i].card);
            // var values =  stats.statsInfo.Values;
            // var keys =  stats.statsInfo.Keys;
            // Debug.Log();
            // cards[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = stats.statsInfo[cardSlot.card.CardStatsSettings[i]]; 
            cards[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "lvl " + cardSlot.level.ToString(); 
            cards[i].transform.GetChild(2).GetComponent<Image>().sprite = cardSlot.card.Icon;
        }

        for (; i < cards.Count; i++)
            cards[i].SetActive(false);
    }
}
