using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
