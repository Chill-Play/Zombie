using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorAI : MonoBehaviour
{
    [SerializeField] Transform modelPivot;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] UnitShooting unitShooting;
    [SerializeField] PlayerResources resources;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(unitShooting.Target != null)
        {
            resources.CanDigResources = false;
            Vector3 direction = unitShooting.Target.transform.position - modelPivot.position;
            direction.y = 0;
            direction.Normalize();
            modelPivot.rotation = Quaternion.RotateTowards(modelPivot.rotation, Quaternion.LookRotation(direction), navMeshAgent.angularSpeed * Time.deltaTime);
            unitShooting.AllowShooting = Vector3.Angle(modelPivot.transform.forward, direction) < 15f;

        }
        else
        {
            resources.CanDigResources = true;
            if (unitShooting.AllowShooting)
            {
                unitShooting.AllowShooting = false;
            }
            modelPivot.localRotation = Quaternion.RotateTowards(modelPivot.localRotation, Quaternion.identity, navMeshAgent.angularSpeed * Time.deltaTime);
        }
    }
}
