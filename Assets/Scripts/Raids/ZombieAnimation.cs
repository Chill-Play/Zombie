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
    [SerializeField] int attackAnimationsCount;

    private void Awake()
    {
        health.OnDead += Health_OnDead;
        GetComponent<UnitMeleeFighting>().OnAttack += ZombieAnimation_OnAttack;
    }


    private void Health_OnDead(EventMessage<Empty> obj)
    {
        //animator.Play(1.0f);
        animator.SetTrigger("Death");
        animator.SetInteger("DeathId", Random.Range(0, deathAnimationsCount));
    }

    private void Update()
    {
        animator.SetFloat("MovementSpeed", movement.Velocity.magnitude / agent.speed);
    }


    private void ZombieAnimation_OnAttack()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("AttackId", Random.Range(0, attackAnimationsCount));
    }
}
