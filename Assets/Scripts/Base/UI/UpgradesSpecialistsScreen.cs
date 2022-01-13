using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradesSpecialistsScreen : UIScreen, IShowScreen
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text label;
    [SerializeField] List<StatUpgradeCard> cards;
    [SerializeField] Transform panel;

    List<(StatsType, StatInfo)> stats = new List<(StatsType, StatInfo)>();
    ResourcesInfo availableResources;
    System.Action onClose;
    
    public void Show(UpgradeZone zone, string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, Action onClose = null)
    {
        this.onClose = onClose;
        label.text = name;
        this.stats = stats;
        this.availableResources = availableResources;
    }
}
