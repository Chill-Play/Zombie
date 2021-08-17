using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatInfo
{
    public StatsType statsType;
    public int value;

}

public class StatsManager : SingletonMono<StatsManager>
{
    [SerializeField] List<StatInfo> baseStats = new List<StatInfo>();

    Dictionary<StatsType, int> statsInfo = new Dictionary<StatsType, int>();

    public Dictionary<StatsType, int> StatsInfo => statsInfo;

    private void Awake()
    {
        for (int i = 0; i < baseStats.Count; i++)
        {
            statsInfo.Add(baseStats[i].statsType, baseStats[i].value);
        }
    }

    public int AddStat(StatsType statsType, int value = 1)
    {
        if (statsInfo.ContainsKey(statsType))
        {
            statsInfo[statsType]++;
            return (statsInfo[statsType]);
        }
        return -1;
    }
}
