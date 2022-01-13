using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShowScreen
{
    void Show(UpgradeZone zone, string name, List<(StatsType, StatInfo)> stats, ResourcesInfo availableResources, System.Action onClose = null);
}