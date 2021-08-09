using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool IsReturningToBase { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if(IsReturningToBase)
        {
            Level.Instance.EndLevel();
        }
    }
}
