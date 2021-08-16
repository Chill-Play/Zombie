using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuildingReport
{
    public bool resourcesUsed;
    public bool buildingFinished;
    public bool needToResetSpeed;

    public BuildingReport(bool resourcesUsed, bool buildingFinished, bool needToResetSpeed = false)
    {
        this.resourcesUsed = resourcesUsed;
        this.buildingFinished = buildingFinished;
        this.needToResetSpeed = false;
    }
}

public interface IBuilding 
{

}
