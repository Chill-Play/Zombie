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
    [SerializeField] UnitTargetDetection unitTargetDetection;
    [SerializeField] Vector3 spineAngleOffset;


    // Start is called before the first frame update
    void OnEnable()
    {
        if (shooting != null)
        {
            shooting.OnShoot += Shooting_OnShoot;
        }
    }


    private void OnDisable()
    {
        if (shooting != null)
        {
            shooting.OnShoot -= Shooting_OnShoot;
        }
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
        if (shooting != null)
        {
            if (unitTargetDetection.Target != null)
            {
                Vector3 aimAngle = modelPivot.transform.eulerAngles + spineAngleOffset;
                aimSpineBone.eulerAngles = aimAngle;
            }
        }
    }


    public void SetInteraction(ResourceInteractionType type, bool play)
    {
        switch (type)
        {
            case ResourceInteractionType.Crouching:
                animator.SetBool("Crouching", play);
                break;
            case ResourceInteractionType.Swinging:
                animator.SetBool("Swinging", play);
                break;
        }
        animator.SetLayerWeight(1, 0f);
    }


    public void ResetInteraction()
    {
        animator.SetBool("Crouching", false);
        animator.SetBool("Swinging", false);
        animator.SetLayerWeight(1, 1f);
    }
}
