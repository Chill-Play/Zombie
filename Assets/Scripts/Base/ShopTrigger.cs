using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour //ONLY FOR FIRST BUILD 
{
    [SerializeField] GameObject go;
    void OnTriggerEnter(Collider other)
    {
        go.SetActive(true);
    }
}
