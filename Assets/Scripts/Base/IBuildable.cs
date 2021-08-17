using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildable
{
    event System.Action<List<CostInfo>> OnInitialized;
    event System.Action<Dictionary<ResourceType, int>> OnUpdated;


    BuildingReport TryUseResources(List<ResourceType> playerResources, int count);
}
