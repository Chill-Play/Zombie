using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class UnitDash : UnitFighting
{
    [SerializeField] private TrajectoryRenderer directionLine;
    [SerializeField] private float aimTime = 1;
    [SerializeField] private float cooldown = 3;
    [SerializeField] private float dashDistance;
    
    UnitTargetDetection unitTargetDetection;
    private UnitBodyHit bodyHit;
    private ZombieMovement movement;
    private Vector3 cameraPos;
    private UnitHealth unitHealth;
    private float normalSpeed;

    private void Awake()
    {
        directionLine.gameObject.SetActive(false);
        unitTargetDetection = GetComponent<UnitTargetDetection>();
        bodyHit = GetComponent<UnitBodyHit>();
        movement = GetComponent<ZombieMovement>();
        unitHealth = GetComponent<UnitHealth>();
        unitHealth.OnDead += Stop;
        bodyHit.enabled = false;
        Attacking = false;
    }

    private void Update()
    {
        if (unitTargetDetection.Target != null)
        {
            if (!Attacking)
            {
                Debug.Log("Hello world");
                Vector3 direction = (unitTargetDetection.Target.position - transform.position).normalized;
                StartCoroutine(OnDashCoroutine(direction));
                Attacking = true;
            }
        }
    }

    IEnumerator OnDashCoroutine(Vector3 direction)
    {
        transform.LookAt(unitTargetDetection.Target);
        Debug.Log("StopMoving");
        directionLine.gameObject.SetActive(true);
        directionLine.ShowTrajectory(transform.position, transform.position+ direction * dashDistance);
        Debug.Log("directionLine.SetActive(true);");
        movement.Agent.enabled = false;
        yield return new WaitForSeconds(aimTime);
        bodyHit.enabled  = true;
        directionLine.gameObject.SetActive(false);
        transform.DOMove(transform.position + direction * dashDistance, 1);
        //MoveTo(transform.position + (direction * dashDistance));
        Debug.DrawLine(transform.position, transform.position + direction * dashDistance, Color.red, float.MaxValue);
        Debug.Log("MoveTo");
        yield return new WaitForSeconds(cooldown);
        movement.Agent.enabled = true;
        Attacking = false;
    }


    void Stop(EventMessage<Empty> empty)
    {
        directionLine.gameObject.SetActive(false);
        StopAllCoroutines();
    }

}