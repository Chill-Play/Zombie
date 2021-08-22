using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitMovement : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;

    public bool InputActive => Input.magnitude > 0.05f;
    public Vector2 Input { get; set; }

    public abstract void MoveTo(Vector3 target);

    public abstract void StopMoving();
}
