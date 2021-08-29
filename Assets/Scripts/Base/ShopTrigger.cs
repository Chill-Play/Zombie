using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour //ONLY FOR FIRST BUILD 
{
    [SerializeField] GameObject go;
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBuilding>() != null)
        {
            go.SetActive(true);
        }
    }
}
