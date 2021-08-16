using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorAI : MonoBehaviour
{
    [SerializeField] Transform modelPivot;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] UnitTargetDetection targetDetection;
    [SerializeField] UnitShooting unitShooting;
    [SerializeField] InteractivePointDetection interactivePointDetection;
    [SerializeField] StateController stateController;
    [SerializeField] SubjectId movingState;
    [SerializeField] SubjectId shootingState;
    [SerializeField] SubjectId interactingState;

    Squad squad;

    private void Start()
    {
        squad = FindObjectOfType<Squad>();
    }

    void Update()
    {       
        if (targetDetection.Target != null)
        {
            ToState(shootingState);
            Vector3 direction = targetDetection.Target.transform.position - modelPivot.position;
            direction.y = 0;
            direction.Normalize();
            modelPivot.rotation = Quaternion.RotateTowards(modelPivot.rotation, Quaternion.LookRotation(direction), navMeshAgent.angularSpeed * Time.deltaTime);
            unitShooting.AllowShooting = Vector3.Angle(modelPivot.transform.forward, direction) < 15f;
        }
        else if (squad.IsMoving)
        {        
            ToState(movingState);
            RotateToward();
        }
        else if (interactivePointDetection.Target != null)
        {
            ToState(interactingState);
            RotateToward();
        }
        else
        {
            ToState(movingState);
            RotateToward();
        }
    }

    void ToState(SubjectId state)
    {
        if (stateController.CurrentStateId != state)
        {           
            stateController.ToState(state);
        }
    }

    void RotateToward()
    {
        modelPivot.localRotation = Quaternion.RotateTowards(modelPivot.localRotation, Quaternion.identity, navMeshAgent.angularSpeed * Time.deltaTime);
    }
}
