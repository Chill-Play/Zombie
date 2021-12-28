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
    [SerializeField] UnitHealth unitHealth;
    [SerializeField] InteractivePointDetection interactivePointDetection;
    [SerializeField] StateController stateController;
    [SerializeField] Collider mainCollider;
    [SerializeField] SubjectId movingState;
    [SerializeField] SubjectId shootingState;
    [SerializeField] SubjectId interactingState;
    [SerializeField] SubjectId deadState;
    [SerializeField] SubjectId leaderDefeatedState;

    Squad squad;
    Level level;

    Constructive constructive;
    Repairable repairable;

    bool construction = false;

    private void Start()
    {
        level = FindObjectOfType<Level>();
        squad = FindObjectOfType<Squad>();
        unitHealth.OnDead += UnitHealth_OnDead;
        level.OnLevelFailed += SurvivorAI_OnLevelFailed;
        interactivePointDetection.OnTargetChanged += InteractivePointDetection_OnTargetChanged;       
    }

    private void InteractivePointDetection_OnTargetChanged(InteractivePoint lastPoint, InteractivePoint point)
    {
        if (point != null)
        {
            constructive = point.gameObject.GetComponent<Constructive>();
            repairable = point.gameObject.GetComponent<Repairable>();
        }
        else
        {
            constructive = null;
            repairable = null;
        }
    }

    private void SurvivorAI_OnLevelFailed()
    {        
        ToState(leaderDefeatedState);
    }

    private void UnitHealth_OnDead(EventMessage<Empty> obj)
    {
        mainCollider.enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        level.OnLevelFailed -= SurvivorAI_OnLevelFailed;
        ToState(deadState);
        
    }

    void Update()
    {
        if (stateController.CurrentStateId == leaderDefeatedState)
        {
            if (targetDetection.Target != null)
            {
                RotateToTarget();
            }
            return;
        }

        if (interactivePointDetection.Target != null && !squad.IsMoving && targetDetection.Target == null
            && (((constructive != null && constructive.CanConstruct) || (repairable != null && repairable.CanRepair))
            || (constructive == null && repairable == null)))
        {
            ToState(interactingState);
            RotateToward();
        }
        else
        {           
            ToState(movingState);
            if (targetDetection.Target != null)
            {
                RotateToTarget();
            }
        }
    }

    void RotateToTarget()
    {
        Vector3 direction = targetDetection.Target.transform.position - modelPivot.position;
        direction.y = 0;
        direction.Normalize();
        modelPivot.rotation = Quaternion.RotateTowards(modelPivot.rotation, Quaternion.LookRotation(direction), navMeshAgent.angularSpeed * Time.deltaTime);
        unitShooting.AllowShooting = Vector3.Angle(modelPivot.transform.forward, direction) < 15f;
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
