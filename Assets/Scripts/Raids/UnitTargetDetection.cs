using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTargetDetection : MonoBehaviour
{
    [SerializeField] float radius;

    Transform target;
    float nextCheck;
    Collider[] attackColliders = new Collider[30];

    public Transform Target => target;

    void Update()
    {
        if (Time.timeSinceLevelLoad > nextCheck)
        {
            nextCheck = Time.timeSinceLevelLoad + Random.Range(0.3f, 0.5f);
            int collidersFound = Physics.OverlapSphereNonAlloc(transform.position, radius, attackColliders, GameplayUtils.ATTACK_MASK);
            target = GameplayUtils.GetAttackTarget(0, collidersFound, transform, attackColliders);
        }
    }

}
