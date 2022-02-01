using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class UnitDash : UnitFighting
{
    [SerializeField] private GameObject directionLine;
    [SerializeField] private float aimTime = 1;
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private float bodyHitTime = 3;
    [SerializeField] private float dashDistance;
    
    UnitTargetDetection unitTargetDetection;
    private UnitBodyHit bodyHit;
    private ZombieMovement movement;
    private Vector3 cameraPos;
    private UnitHealth unitHealth;
    private float normalSpeed;

    private void Awake()
    {
        directionLine.SetActive(false);
        unitTargetDetection = GetComponent<UnitTargetDetection>();
        bodyHit = GetComponent<UnitBodyHit>();
        movement = GetComponent<ZombieMovement>();
        normalSpeed = movement.Agent.speed;
        unitHealth = GetComponent<UnitHealth>();
        unitHealth.OnDead += Stop;
        bodyHit.enabled = false;
    }

    private void Update()
    {
        if (unitTargetDetection.Target != null)
        {
            if (!Attacking)
            {
                Debug.Log("Hello world");
                StartCoroutine(OnDashCoroutine());
                Attacking = true;
            }
        }
        else
        {
            Attacking = false;
        }
    }
    
    IEnumerator OnDashCoroutine()
    {
        movement.StopMoving();
        Debug.Log("StopMoving");
        directionLine.SetActive(true);
        Debug.Log("directionLine.SetActive(true);");
        yield return new WaitForSeconds(aimTime);
        bodyHit.enabled  = true;
        directionLine.SetActive(false);
        movement.MoveTo(transform.position + (transform.forward * dashDistance));
        Debug.Log("MoveTo");
        movement.Agent.speed = dashSpeed;
        while (!movement.IsReachDestination)
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("IsReachDestination");
        yield return new WaitForSeconds(bodyHitTime);
        movement.Agent.speed = normalSpeed;
        Attacking = false;
    }


    void Stop(EventMessage<Empty> empty)
    {
        directionLine.SetActive(false);
        StopAllCoroutines();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * dashDistance);
    }
}