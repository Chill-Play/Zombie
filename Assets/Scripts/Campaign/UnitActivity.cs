using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActivity : MonoBehaviour
{
    [SerializeField] float useRadius = 2f;
    [SerializeField] LayerMask useMask;

    protected Collider[] useSpots = new Collider[3];
    protected int count = 0;


    protected virtual void Awake()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, useRadius, useSpots, useMask);      
      
    }
}
