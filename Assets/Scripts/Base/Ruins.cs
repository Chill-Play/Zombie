using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruins : MonoBehaviour
{
    [SerializeField] GameObject buildingRuins;


    public void Show(bool show)
    {
        buildingRuins.gameObject.SetActive(show);
    }
}
