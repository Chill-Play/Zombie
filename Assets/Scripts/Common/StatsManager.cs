using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatInfo
{
    public int level;

}

public class StatsManager : SingletonMono<StatsManager>
{
    [SerializeField] List<StatsType> stats = new List<StatsType>();

    Dictionary<StatsType, StatInfo> statsInfo = new Dictionary<StatsType, StatInfo>();

    private void Awake()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            statsInfo.Add(stats[i], new StatInfo());
        }
    }

    public int AddStat(StatsType statsType, int value = 1)
    {
        if (statsInfo.ContainsKey(statsType))
        {
            statsInfo[statsType].level += value;
            return (statsInfo[statsType].level);
        }
        Debug.LogError("No such stat : " + statsType.name);
        return -1;
    }


    public StatInfo GetStatInfo(StatsType type)
    {
        return statsInfo[type];
    }
}
