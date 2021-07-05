using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    [SerializeField] LayerMask buildingsMask;
    [SerializeField] float radius;
    [SerializeField] Player player;
    [SerializeField] int countPerUse = 10;
    [SerializeField] float baseRate = 0.25f;
    [SerializeField] float rateIncrease = 0.01f;


    Collider[] buildings = new Collider[1];
    float nextUse;
    int uses;

    void FixedUpdate()
    {
        if (player.InputActive)
        {
            return;
        }
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buildings, buildingsMask);
        if (count > 0)
        {
            if (nextUse < Time.time)
            {
                Building building = buildings[0].GetComponent<Building>();
                bool used = building.TryUseResources(ResourcesController.Instance.Resources, countPerUse);
                if (used)
                {
                    uses++;
                    nextUse = Time.time + baseRate - (rateIncrease * uses);
                }
                else
                {
                    uses = 0;
                }
            }
        }
        else
        {
            uses = 0;
        }
    }
}
