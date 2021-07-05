using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ResourceInteractionType
{
    Crouching,
    Swinging
}


public class PlayerAnimation : MonoBehaviour
{
    
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    [SerializeField] InverseKinematics rightIkSolver;
    [SerializeField] InverseKinematics leftIkSolver;

    bool handIKActive;

    public bool HandIKActive
    {
        get
        {
            return handIKActive;
        }
        set
        {
            rightIkSolver.enabled = value;
            leftIkSolver.enabled = value;
            handIKActive = value;
        }
    }
    public Vector3 LeftIKTarget { get; set; }
    public Vector3 RightIKTarget { get; set; }

    void Start()
    {
        
    }


    void Update()
    {
        Vector3 relativeVelocity = agent.transform.InverseTransformVector(agent.velocity);
        relativeVelocity /= agent.speed;
        animator.SetFloat("MovementSpeedY", relativeVelocity.z);
        animator.SetFloat("MovementSpeedX", relativeVelocity.x);

        if(HandIKActive)
        {
            UpdateIK(rightIkSolver, RightIKTarget);
            UpdateIK(leftIkSolver, LeftIKTarget);
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
    }


    public void ResetInteraction()
    {
        animator.SetBool("Crouching", false);
        animator.SetBool("Swinging", false);
    }


    void UpdateIK(InverseKinematics ik, Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        ik.target.position = transform.position + (direction * 2f);
        ik.target.forward = direction;
    }


}
