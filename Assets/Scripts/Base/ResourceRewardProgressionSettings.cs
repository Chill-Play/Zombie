using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Resource Reward Progression")]
public class ResourceRewardProgressionSettings : ScriptableObject
{
    public ResourceType resourceType;
    public int baseCost;
    public float costMultiplier;
    public float costPower;

    public int GetLevelCost(int level)
    {
        return MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
    }
}
