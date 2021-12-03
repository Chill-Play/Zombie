using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AgroActivator agroActivator = other.GetComponent<AgroActivator>();
        if (agroActivator != null)
        {
            agroActivator.GoAgro();
        }
    }
}
