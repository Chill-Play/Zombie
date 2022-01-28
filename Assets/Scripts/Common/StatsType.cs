using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ZombieSim/Stats Type")]
public class StatsType : ScriptableObject
{
    public string saveId;
    public string displayName;
    public string shortName;
    public Sprite icon;
    public Sprite lockedIcon;
    public List<ResourcesCostProgression> resourcesCostProgression = new List<ResourcesCostProgression>();

    public ResourcesInfo GetLevelCost(int level)
    {
        return MetaUtils.GetLevelCost(level, resourcesCostProgression);
    }
}
