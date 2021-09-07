using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnlockableBuilding : MonoBehaviour
{
    [SerializeField] int unlockLevel;
    [SerializeField] LockerTag lockerPrefab;
    [SerializeField] GameObject obstacle;
    BuildingProcess buildingProcess;
    Buildable buildable;

    LockerTag lockerInstance;
    Transform spot;

    private void Start()
    {
        buildable = GetComponent<Buildable>();
        buildingProcess = GetComponent<BuildingProcess>();
        spot = buildingProcess.BuildingSpot.transform;
        var lockPos = spot.transform.position;
        lockPos.y = 0.4f;
        lockerInstance = Instantiate(lockerPrefab, lockPos, Quaternion.identity, spot);
        lockerInstance.SetLevel(unlockLevel);
    }



    public void SetLevel(int level)
    {
        if (level >= unlockLevel - 1)
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
        if (obstacle != null)
        {
            obstacle.SetActive(!locked);
        }
        if (spot.TryGetComponent<Ruins>(out var ruins))
        {
            ruins.Show(!locked);
        }
        var obstacles = GetComponentsInChildren<NavMeshObstacle>();
        foreach(var obstacle in obstacles)
        {
            obstacle.enabled = !locked;
        }
        buildable.enabled = !locked;
        lockerInstance.gameObject.SetActive(locked);
    }
}
