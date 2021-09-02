using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableBuilding : MonoBehaviour
{
    [SerializeField] int unlockLevel;
    [SerializeField] LockerTag lockerPrefab;
    BuildingProcess buildingProcess;
    Buildable buildable;

    LockerTag lockerInstance;

    private void Start()
    {
        buildable = GetComponent<Buildable>();
        buildingProcess = GetComponent<BuildingProcess>();
        var spot = buildingProcess.BuildingSpot.transform;
        lockerInstance = Instantiate(lockerPrefab, spot.transform.position, Quaternion.identity, spot);
    }



    public void SetLevel(int level)
    {
        if (level >= unlockLevel)
        {
            SetLock(false);
        }
        else
        {
            SetLock(true);
        }
    }


    void SetLock(bool locked)
    {
        buildable.enabled = !locked;
        lockerInstance.gameObject.SetActive(locked);
    }
}
