using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitMovement : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;

    public bool InputActive => Input.magnitude > 0.05f;
    public Vector2 Input { get; set; }

    public bool IsReachDestination => agent.hasPath && agent.remainingDistance <= agent.stoppingDistance;    

    public bool VelocityActive => agent.velocity.magnitude <= 0.05f;

    public abstract void MoveTo(Vector3 target);

    public abstract void StopMoving();

    public abstract bool CanReachDestination(Vector3 destination);   
}
