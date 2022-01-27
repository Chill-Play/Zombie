using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorCampAI : MonoBehaviour
{
    [SerializeField] StateController stateController;
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] UnitInteracting unitInteracting;

    [SerializeField] SubjectId movingStateId;
    [SerializeField] SubjectId interactingStateId;

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
        else
        {
            stateController.ToState(interactingStateId);
        }
    }

}
