using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorMovement : UnitMovement
{
    public bool InputActive => Input.magnitude > 0.05f;
    public bool goToDestination;

    void Update()
    {
        if (!goToDestination)
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
    }


    public override void MoveTo(Vector3 target)
    {
        goToDestination = true;
        Input = Vector2.zero;
        agent.SetDestination(target);
    }

    public override void StopMoving()
    {
        goToDestination = false;
        agent.velocity = Vector3.zero;
    }
}
