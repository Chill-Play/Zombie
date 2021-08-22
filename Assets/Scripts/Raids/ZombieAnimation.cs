using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimation : MonoBehaviour
{
    [SerializeField] UnitHealth health;
    [SerializeField] ZombieMovement movement;
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int deathAnimationsCount;

    private void Awake()
    {
        health.OnDead += Health_OnDead;
    }

    private void Health_OnDead(EventMessage<Empty> obj)
    {
        animator.SetTrigger("Death");
        animator.SetInteger("DeathId", Random.Range(0, deathAnimationsCount));
    }

    private void Update()
    {
        animator.SetFloat("MovementSpeed", movement.Velocity.magnitude / agent.speed);
    }
}
