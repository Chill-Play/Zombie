using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidZone : MonoBehaviour
{
    public event System.Action OnEnterZone;
    public event System.Action OnExitZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBuilding playerBuilding))
        {
           OnEnterZone?.Invoke();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerBuilding playerBuilding))
        {
           OnExitZone?.Invoke();
        }
    }
}
