using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class UpgradesSpecialistsScreen : UIScreen, IShowScreen
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text label;
    private CardController cardController;
    private List<GameObject> cards = new List<GameObject>();
    [SerializeField] Transform panel;

    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;

    private void Awake()
    {
        cardController = FindObjectOfType<CardController>();
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
        Debug.Log("Cards count: "+cardController.ActiveCards.Count);
    }
}
