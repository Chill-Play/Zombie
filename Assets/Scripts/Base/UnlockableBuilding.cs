using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class UnlockableBuilding : MonoBehaviour
{
    [SerializeField] int unlockLevel;
    [SerializeField] LockerTag lockerPrefab;
    [SerializeField] GameObject obstacle;
    BuildingProcess buildingProcess;
    Buildable buildable;

    LockerTag lockerInstance;
    Transform spot;
    bool locked = true;

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

    public bool CanUnlock(int level)
    {
        return level >= unlockLevel - 1 && locked;
    }

    public void UnlockWhithAnimation(System.Action callback = null)
    {
        //Add animation unlock
        var seq = DOTween.Sequence();
        seq.Append(lockerInstance.transform.DOPunchScale(Vector3.one * 0.2f,1, 5, 1).OnComplete(() =>
        {
            lockerInstance.Unlock();
        }));
        seq.AppendInterval(.4f).OnComplete(() =>
        {
            UnlockRuinAnimation(callback);            
        });
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

    void UnlockRuinAnimation(System.Action callback = null)
    {
        if (spot.TryGetComponent<Ruins>(out var ruins))
        {
            ruins.ShowWhithAnimation(callback);
        }
        SetLock(false);
    }


    void SetLock(bool locked)
    {
        this.locked = locked;
        if (obstacle != null)
        {
            obstacle.SetActive(!locked);
        }
        if (spot.TryGetComponent<Ruins>(out var ruins))
        {
            ruins.Show(!locked);
        }
        var obstacles = GetComponentsInChildren<NavMeshObstacle>();
        foreach (var obstacle in obstacles)
        {
            obstacle.enabled = !locked;
        }
        buildable.enabled = !locked;
        lockerInstance.gameObject.SetActive(locked);
    }
}
