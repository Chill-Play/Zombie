using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;
    [SerializeField] Transform aimSpineBone;
    [SerializeField] Transform modelPivot;
    [SerializeField] UnitShooting shooting;
    [SerializeField] Vector3 spineAngleOffset;
    // Start is called before the first frame update
    void OnEnable()
    {
        shooting.OnShoot += Shooting_OnShoot;
    }


    private void OnDisable()
    {
        shooting.OnShoot -= Shooting_OnShoot;
    }

    private void Shooting_OnShoot()
    {
        animator.Play("Shoot", 2, 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativeVelocity = animator.transform.InverseTransformVector(navMeshAgent.velocity);
        relativeVelocity /= navMeshAgent.speed;
        animator.SetFloat("Movement Y", relativeVelocity.z);
        animator.SetFloat("Movement X", relativeVelocity.x);
    }


    void LateUpdate()
    {
        Vector3 aimAngle = modelPivot.transform.eulerAngles + spineAngleOffset;
        aimSpineBone.eulerAngles = aimAngle;
    }
}
