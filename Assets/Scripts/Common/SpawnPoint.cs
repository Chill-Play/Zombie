using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public event System.Action OnReturnedToBase;
    public bool IsReturningToBase { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if(IsReturningToBase)
        {
            OnReturnedToBase?.Invoke();
            //Level.Instance.EndLevel();
        }
    }
}
