using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Stats Type")]
public class StatsType : ScriptableObject
{
    public string saveId;
    public string displayName;
    public Sprite icon;
    public Sprite lockedIcon;
    public ResourcesInfo baseCost;
    public float costMultiplier;
    public float costPower;


    public ResourcesInfo GetLevelCost(int level)
    {
        return MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
    }
}
