using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTargetVolume : MonoBehaviour
{
    public static event System.Action<IBuildable> OnBuildingTargetEnter;
    public static event System.Action<IBuildable> OnBuildingTargetExit;

    IBuildable building;

    private void Awake()
    {
        building = GetComponentInParent<IBuildable>();  
    }

    private void OnTriggerEnter(Collider other)
    {    
        if (other.GetComponent<PlayerBuilder>())
        {
           
            OnBuildingTargetEnter?.Invoke(building);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerBuilder>())
        {
            OnBuildingTargetExit?.Invoke(building);
        }       
    }
}
