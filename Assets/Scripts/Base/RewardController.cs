using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : SingletonMono<RewardController>
{
    const string RESOURCE_PREFS = "M_Reward_Resource_";

    [SerializeField] List<ResourceRewardProgressionSettings> resourceRewardSettings = new List<ResourceRewardProgressionSettings>();

    Dictionary<ResourceType, (int, ResourceRewardProgressionSettings)> resourceRewardLevel = new Dictionary<ResourceType, (int, ResourceRewardProgressionSettings)>();

    private void Awake()
    {
        Load();
    }

    public int GetResourcesRewardCount(ResourceType resourceType)
    {
        var value = resourceRewardLevel[resourceType];       
        return value.Item2.GetLevelCost(value.Item1);
    }

    public void AddResourceRewardLevel(ResourceType resourceType)
    {
        var value = resourceRewardLevel[resourceType];
        resourceRewardLevel[resourceType] = (value.Item1 + 1, value.Item2);
        Save();
    }

    void Save()
    {
        foreach (var setting in resourceRewardLevel)
        {
            PlayerPrefs.SetInt(RESOURCE_PREFS + setting.Key.saveId, setting.Value.Item1);
        }
    }

    void Load()
    {
        resourceRewardLevel.Clear();
        foreach (var setting in resourceRewardSettings)
        {
            int level = PlayerPrefs.GetInt(RESOURCE_PREFS + setting.resourceType.saveId, 0);
            resourceRewardLevel.Add(setting.resourceType, (level, setting));
        }
    }
}
