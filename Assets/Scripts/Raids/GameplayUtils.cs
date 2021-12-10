using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayUtils
{
    public const int OBSTACLE_MASK = 1025;
    public const int ATTACK_MASK = 64;


    public static Transform GetAttackTarget(int side, int collidersCount, Transform transform, Collider[] attackColliders)
    {
        float minDistance = Mathf.Infinity;
        Transform closest = null;
        for (int i = 0; i < collidersCount; i++)
        {
            if (attackColliders[i] == null)
                break;
            Transform target = attackColliders[i].transform;
            float angle = Vector3.SignedAngle(transform.forward, target.position - transform.position, Vector3.up);
            float distance = Vector3.Distance(transform.position, target.position);
            if (!Physics.Linecast(transform.position + Vector3.up * 0.5f, target.transform.position + Vector3.up * 0.5f, OBSTACLE_MASK))
            {               
                if (side != 0 && Mathf.Sign(angle) != side)
                {
                    continue;
                }
                if (minDistance > distance)
                {
                    minDistance = distance;
                    closest = target.transform;
                }
            }
        }
        return closest;
    }
}
