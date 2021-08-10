using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    public Vector2 Input { get; set; }
    public bool InputActive => Input.magnitude > 0.05f;
    public bool goToDestination;

    void Update()
    {
        if (InputActive)
        {            
            agent.velocity = new Vector3(Input.x, 0f, Input.y) * agent.speed;
        }
        else if(!goToDestination)
        {
            agent.velocity = Vector3.zero;
        }
    }


    public void MoveTo(Vector3 target)
    {
        goToDestination = true;
        Input = Vector2.zero;
        agent.SetDestination(target);
    }

    public bool GetNearestPoint(Vector3 target, float distance, out Vector3 result)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, distance, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = target;
        return false;
    }

    public Vector3 CalculateNextPoint(Vector2 input)
    {       
        return (transform.position + new Vector3(input.x, 0f, input.y) * agent.speed);
    }

    public bool IsBlocked()
    {
        return agent.velocity.magnitude < 0.3f;            
    }
}
