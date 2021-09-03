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

    public int AddStatLevel(StatsType statsType, int value = 1)
    {
        if (statsInfo.ContainsKey(statsType))
        {
            var info = statsInfo[statsType];
            info.level += value;
            var key = GetStatSaveId(statsType);
            PlayerPrefs.SetInt(key, info.level);
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
        if (type != null)
        {
            Debug.Log("Stat type : " + type.name);
        }
        else
        {
            Debug.Log("Stat type is null");
        }
        return statsInfo[type];
    }
}
