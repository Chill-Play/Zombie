using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorCampAI : MonoBehaviour
{
    [SerializeField] StateController stateController;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] UnitInteracting unitInteracting;
    [SerializeField] private UnitPlayerInput playerInput;

    [SerializeField] SubjectId movingStateId;
    [SerializeField] SubjectId interactingStateId;
    [SerializeField] private SubjectId idleStateId;
    
    private void Awake()
    {
        playerInput.OnInputDisable += GoIdle;
    }

    private void Start()
    {
        unitInteracting.CanMoveToResources = false;
    }

    private void Update()
    {
        if (unitMovement.InputActive)
        {
            stateController.ToState(movingStateId);
        }
        else if (stateController.CurrentStateId != idleStateId)
        {
            stateController.ToState(interactingStateId);
        }
    }

    public void GoIdle()
    {
        stateController.ToState(idleStateId);
    }
}
