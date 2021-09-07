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
    const string SAVE_PREFS_PREFIX = "M_Stat_";

    public event System.Action<StatsType, int> OnStatsUpdate;

    [SerializeField] List<StatsType> stats = new List<StatsType>();

    Dictionary<StatsType, StatInfo> statsInfo = new Dictionary<StatsType, StatInfo>();

    private void Awake()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            var info = new StatInfo();
            var key = GetStatSaveId(stats[i]);
            info.level = PlayerPrefs.GetInt(key, 0);
            statsInfo.Add(stats[i], info);          
        }
    }

    private void Start()
    {
        foreach (var stat in statsInfo)
        {
            OnStatsUpdate?.Invoke(stat.Key,stat.Value.level);
        }
    }

    public int AddStatLevel(StatsType statsType, int value = 1)
    {
        if (statsInfo.ContainsKey(statsType))
        {
            var info = statsInfo[statsType];
            info.level += value;
            var key = GetStatSaveId(statsType);
            PlayerPrefs.SetInt(key, info.level);
            OnStatsUpdate?.Invoke(statsType, info.level);
            return info.level;
        }
        Debug.LogError("No such stat : " + statsType.name); 
        return -1;
    }


    string GetStatSaveId(StatsType type)
    {
        return SAVE_PREFS_PREFIX + type.saveId;
    }


    public StatInfo GetStatInfo(StatsType type)
    {
        return statsInfo[type];
    }
}
