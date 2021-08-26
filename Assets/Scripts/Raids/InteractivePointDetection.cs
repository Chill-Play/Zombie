using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePointDetection : MonoBehaviour
{
    [SerializeField] float detectionRadius = 2f;
    [SerializeField] LayerMask interactivePointMask;

    InteractivePoint target; 
    Collider[] interactivePoints = new Collider[3];

   public  InteractivePoint Target => target;  

    void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, interactivePoints, interactivePointMask);

        float minDist = float.MaxValue;

        if (count > 0)
        {
            //if (target == null)
            //{
                for (int i = 0; i < interactivePoints.Length; i++)
                {
                    if (interactivePoints[i] != null)
                    {
                        InteractivePoint possibleTarget = interactivePoints[i].GetComponent<InteractivePoint>();
                        if (possibleTarget.HasFreePoint())
                        {                           
                            float dist = Vector3.Distance(interactivePoints[i].transform.position, transform.position);
                            if (minDist > dist)
                            {
                                minDist = dist;
                                target = possibleTarget;
                            }

                        }
                    }
                }
            //}
        }
        else
        {
            target = null;
        }
      
    }

    public void NullTarget()
    {
        target = null;
    }
}
