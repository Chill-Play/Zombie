using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Vector3 vellR;
    [SerializeField] Vector3 vell;

    public Vector2 Input { get; set; }
    public bool InputActive => Input.magnitude > 0.05f;
    public bool goToDestination;

    void Update()
    {
        if (!goToDestination)
        {
          
            if (InputActive)
            {
                //agent.isStopped = false;
                agent.velocity = new Vector3(Input.x, 0f, Input.y) * agent.speed;
                vell = new Vector3(Input.x, 0f, Input.y) * agent.speed;
                vellR = agent.velocity;
            }
            else 
            {
                agent.velocity = Vector3.zero;
            }
           
        }
    }


    public void MoveTo(Vector3 target)
    {       
        goToDestination = true;
        Input = Vector2.zero;
        agent.SetDestination(target);
    }

    public void StopMoving()
    {       
        goToDestination = false;
        if(agent.isOnNavMesh)
        agent.ResetPath();        
    }
}
