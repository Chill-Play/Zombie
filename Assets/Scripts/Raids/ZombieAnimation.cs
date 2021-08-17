using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;

    private void Update()
    {
        animator.SetFloat("MovementSpeed", agent.velocity.magnitude / agent.speed);
    }
}
