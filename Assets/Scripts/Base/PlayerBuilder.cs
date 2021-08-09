using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{   
    [SerializeField] int countPerUse = 10;
    [SerializeField] float baseRate = 0.25f;
    [SerializeField] float rateIncrease = 0.01f;
    
    float nextUse;
    int uses;

    IBuilding targetBuilding;
    UnitMovement unitMovement;

    private void OnEnable()
    {
        unitMovement = GetComponent<UnitMovement>();
        BuildingTargetVolume.OnBuildingTargetEnter += BuildingTargetVolume_OnBuildingTargetEnter;
        BuildingTargetVolume.OnBuildingTargetExit += BuildingTargetVolume_OnBuildingTargetExit;
    }
 
    private void BuildingTargetVolume_OnBuildingTargetEnter(IBuilding obj)
    {
        targetBuilding = obj;
    }

    private void BuildingTargetVolume_OnBuildingTargetExit(IBuilding obj)
    {       
        if (targetBuilding == obj)
        {
            targetBuilding = null;
        }
    }

    void FixedUpdate()
    {
        if (!unitMovement.InputActive)
        {

            if (targetBuilding != null)
            {
                if (nextUse < Time.time)
                {
                    BuildingReport buildingReport = targetBuilding.TryUseResources(ResourcesController.Instance.Resources, countPerUse);
                    if (buildingReport.resourcesUsed)
                    {
                        uses++;
                        nextUse = Time.time + baseRate - (rateIncrease * uses);
                    }
                    else
                    {
                        uses = 0;
                    }
                    if (buildingReport.needToResetSpeed)
                    {
                        uses = 0;
                    }
                    if (buildingReport.buildingFinished)
                    {
                        targetBuilding = null;
                    }
                }
            }
            else
            {
                uses = 0;
            }
        }
    }


    private void OnDisable()
    {
        BuildingTargetVolume.OnBuildingTargetEnter -= BuildingTargetVolume_OnBuildingTargetEnter;
        BuildingTargetVolume.OnBuildingTargetExit -= BuildingTargetVolume_OnBuildingTargetExit; ;
    }
}
