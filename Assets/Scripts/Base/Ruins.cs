using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruins : MonoBehaviour
{
    [SerializeField] GameObject buildingRuins;


    public void Show(bool show)
    {
        var obstacles = GetComponentsInChildren<UnityEngine.AI.NavMeshObstacle>();
        foreach(var obstacle in obstacles)
        {
            obstacle.enabled = show;
        }
        buildingRuins.gameObject.SetActive(show);
    }
}
