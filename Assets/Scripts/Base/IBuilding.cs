using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuildingReport
{
    public bool resourcesUsed;
    public bool buildingFinished;

    public BuildingReport(bool resourcesUsed, bool buildingFinished)
    {
        this.resourcesUsed = resourcesUsed;
        this.buildingFinished = buildingFinished;
    }
}

public interface IBuilding 
{
    BuildingReport TryUseResources(List<ResourceType> playerResources, int count);
}
