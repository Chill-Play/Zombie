using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePointDetection : MonoBehaviour
{
    [SerializeField] float detectionRadius = 2f;
    [SerializeField] LayerMask interactivePointMask;

    InteractivePoint target; 
    Collider[] interactivePoints = new Collider[1];

   public  InteractivePoint Target => target;

    void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, interactivePoints, interactivePointMask);
        if (count > 0)
        {          
            target = interactivePoints[0].GetComponent<InteractivePoint>();          
        }
        else
        {
            target = null;
        }
    }
}
