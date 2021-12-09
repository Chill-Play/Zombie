using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDetectConstructions : MonoBehaviour
{
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] LayerMask targetMask;

    protected Collider[] targets = new Collider[3];
    protected int count = 0;

    ZombiesTarget target;

    public ZombiesTarget Target => target;

    protected virtual void FixedUpdate()
    {
        target = null;
        count = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, targets, targetMask);
        if (count > 0 && (target == null || !target.enabled))
        {
            float minDist = float.MaxValue;           
            for (int i = 0; i < count; i++)
            {
                float dist = Vector3.Distance(transform.position, targets[i].transform.position);
                if (dist < minDist)
                {
                   
                    ZombiesTarget zombiesTarget = targets[i].GetComponent<ZombiesTarget>();
                    if (zombiesTarget != null && zombiesTarget.CanBeAdded && zombiesTarget.enabled)
                    {
                        target = zombiesTarget;
                        minDist = dist;
                    }
                }
            }        
        }
    }
}
