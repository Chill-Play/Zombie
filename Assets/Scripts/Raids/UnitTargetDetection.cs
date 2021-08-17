using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTargetDetection : MonoBehaviour
{
    [SerializeField] float radius;

    Transform target; 
    Collider[] attackColliders = new Collider[30];

    public Transform Target => target;

    void Update()
    {
        int collidersFound = Physics.OverlapSphereNonAlloc(transform.position, radius, attackColliders, GameplayUtils.ATTACK_MASK);
        target = GameplayUtils.GetAttackTarget(0, collidersFound, transform, attackColliders);      
    }

}
