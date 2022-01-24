using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidZone : MonoBehaviour
{
    public event System.Action OnEnterZone;
    public event System.Action OnExitZone;

    [SerializeField] Transform spawnPoint;

    public Transform SpawnPoint => spawnPoint;

    bool lockTrigger = true;

    private void OnEnable()
    {
        lockTrigger = true;
        StartCoroutine(LockTimer());
    }

    IEnumerator LockTimer()
    {
        yield return new WaitForSeconds(1f);
        lockTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!lockTrigger && other.TryGetComponent(out PlayerBuilding playerBuilding))
        {
           OnEnterZone?.Invoke();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!lockTrigger && other.TryGetComponent(out PlayerBuilding playerBuilding))
        {
           OnExitZone?.Invoke();
        }
    }
}
