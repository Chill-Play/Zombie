using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    public Vector2 Input { get; set; }
    public bool InputActive => Input.magnitude > 0.05f;

    void Update()
    {
        if (InputActive)
        {
            agent.velocity = new Vector3(Input.x, 0f, Input.y) * agent.speed;
        }
        else
        {
            agent.velocity = Vector3.zero;
        }
    }


    public void MoveTo(Vector3 target)
    {
        Input = Vector2.zero;
        agent.SetDestination(target);
    }
}
