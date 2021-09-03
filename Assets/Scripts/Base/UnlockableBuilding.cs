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
    Transform spot;

    private void Start()
    {
        buildable = GetComponent<Buildable>();
        buildingProcess = GetComponent<BuildingProcess>();
        spot = buildingProcess.BuildingSpot.transform;
        lockerInstance = Instantiate(lockerPrefab, spot.transform.position, Quaternion.identity, spot);
        lockerInstance.SetLevel(unlockLevel);
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
        if(spot.TryGetComponent<Ruins>(out var ruins))
        {
            ruins.Show(!locked);
        }
        buildable.enabled = !locked;
        lockerInstance.gameObject.SetActive(locked);
    }
}
