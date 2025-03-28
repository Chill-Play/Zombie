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
            float cost = (float)level * costMultiplier + Mathf.Pow(costPower, level) + slot.count;
            ResourceSlot newSlot = new ResourceSlot(slot.type, (int)cost);
            info.AddSlot(newSlot);
        }
        return info;
    }
}
