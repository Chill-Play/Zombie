using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorUpgradeCounter : UpgradeCounter
{
    [SerializeField] List<StatsType> statTypes = new List<StatsType>();

    StatsManager statsManager;

    protected override void Awake()
    {
        base.Awake();
        statsManager = FindObjectOfType<StatsManager>();
        statsManager.OnStatLevelUp += StatsManager_OnStatLevelUp;
    }

    private void StatsManager_OnStatLevelUp((StatsType, int) obj)
    {
        if (statTypes.Contains(obj.Item1))
        {
            RequireUpdate();
        }
    }

    public override int AvailableUpgrades()
    {
        int result = base.AvailableUpgrades();

        foreach (var statType in statTypes)
        {
            int lvl = statsManager.GetStatInfo(statType).level;
            ResourcesInfo cost = statType.GetLevelCost(lvl);
            if (cost.IsFilled(resourcesController.ResourcesCount))
            {
                result++;
            }
        } 
        return result;
    }
}
