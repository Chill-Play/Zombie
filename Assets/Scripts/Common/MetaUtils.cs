using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourcesCostProgression
{
    public ResourceType resourceType;
    public float costMultiplier;
    public float costPower;
    public int baseCost;
}

public static class MetaUtils 
{
    public static ResourcesInfo GetLevelCost(int level, float costMultiplier, float costPower, ResourcesInfo baseCost)
    {
        ResourcesInfo info = new ResourcesInfo();
        foreach (var slot in baseCost.Slots)
        {
            float cost = (float)level * costMultiplier + Mathf.Pow(costPower, level) * Mathf.Clamp01(level) + slot.count;
            ResourceSlot newSlot = new ResourceSlot(slot.type, (int)cost);
            info.AddSlot(newSlot);
        }
        return info;
    }

    public static int GetLevelCost(int level, float costMultiplier, float costPower, int baseCost)
    {     
            return (int)(level * costMultiplier + Mathf.Pow(costPower, level) * Mathf.Clamp01(level) + baseCost); 
    }

    public static ResourcesInfo GetLevelCost(int level, List<ResourcesCostProgression> resourcesCostProgression)
    {
        ResourcesInfo info = new ResourcesInfo();
        foreach (var slot in resourcesCostProgression)
        {
            float cost = (float)level * slot.costMultiplier + Mathf.Pow(slot.costPower, level) * Mathf.Clamp01(level) + slot.baseCost;
            ResourceSlot newSlot = new ResourceSlot(slot.resourceType, (int)cost);
            info.AddSlot(newSlot);
        }
        return info;
    }
}
