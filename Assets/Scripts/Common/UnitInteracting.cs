using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInteracting : MonoBehaviour
{
    [SerializeField] protected InteractivePointDetection interactivePointDetection;
    [SerializeField] protected UnitMovement unitMovement;

    public bool CanMoveToResources { get; set; } = true;

    protected InteractivePoint.WorkingPoint  target;

    protected virtual void OnEnable()
    {
        TryToFindFreePoint();
        interactivePointDetection.OnTargetChanged += InteractivePointDetection_OnTargetChanged;
    }

    private void InteractivePointDetection_OnTargetChanged(InteractivePoint interactivePoint, InteractivePoint newInteractivePoint)
    {
        if (interactivePoint != null && target.index != -1)
        {
            interactivePoint.FreePoint(target);
        }
        target = new InteractivePoint.WorkingPoint(null, -1);
        TryToFindFreePoint();
    }

    void TryToFindFreePoint()
    {
        if (CanMoveToResources && interactivePointDetection.Target != null)
        {
            target = interactivePointDetection.Target.GetFreePoint(transform.position, unitMovement);

            if (target.transform != null && unitMovement.CanReachDestination(target.transform.position))
            {
                interactivePointDetection.Target.TakePoint(target);
                unitMovement.MoveTo(target.transform.position);
            }
            else
            {               
                interactivePointDetection.NullTarget();
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (target.transform == null)
        {
            TryToFindFreePoint();
        }
    }

    protected virtual void OnDisable()
    {
        interactivePointDetection.OnTargetChanged -= InteractivePointDetection_OnTargetChanged;
        if (target.index != -1)
        {
            if (interactivePointDetection.Target != null)
            {
                interactivePointDetection.Target.FreePoint(target);
            }
        }
        target = new InteractivePoint.WorkingPoint(null, -1);
        unitMovement.StopMoving();
    }
}
