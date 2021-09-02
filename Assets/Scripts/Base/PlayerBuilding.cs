using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    [SerializeField] LayerMask buildablesMask;
    [SerializeField] float radius;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] int countPerUse = 10;
    [SerializeField] float baseRate = 0.25f;
    [SerializeField] float rateIncrease = 0.01f;


    Collider[] buildablesBuffer = new Collider[1];
    float nextUse;
    int uses;

    void FixedUpdate()
    {
        if (unitMovement.InputActive)
        {
            return;
        }
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buildablesBuffer, buildablesMask);
        if (count > 0)
        {
            if (nextUse < Time.time)
            {
                Buildable buildable = buildablesBuffer[0].GetComponent<Buildable>();
                if (buildable != null && buildable.enabled)
                {
                    if (!buildable.Built)
                    {
                        var controller = ResourcesController.Instance;
                        buildable.SpendResources(controller.ResourcesCount, countPerUse);
                        controller.UpdateResources();
                        uses++;
                        nextUse = Time.time + baseRate - (rateIncrease * uses);
                    }
                    else
                    {
                        uses = 0;
                    }
                }
            }
        }
        else
        {
            uses = 0;
        }
    }
}
