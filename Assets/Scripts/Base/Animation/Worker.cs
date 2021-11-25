using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] float stopDistance = 0.1f;
    [SerializeField] GameObject carrot;

    public bool IsMoving()
    {
        float remainingDistance = Vector3.Distance(transform.position, agent.destination);

        while(remainingDistance >= stopDistance)
        {
            remainingDistance = Vector3.Distance(transform.position, agent.destination);
            return true;
        }

        return false;
    }

    public void GoToPosition(Vector3 newPosition)
    {
        agent.SetDestination(newPosition);
    }

    public void Work(bool isWork)
    {
        animator.SetBool("Carring", false);
        animator.SetBool("Work", isWork);
        carrot.SetActive(false);
    }

    public void Carring(bool isCarring)
    {
        animator.SetBool("Carring", isCarring);
        carrot.SetActive(isCarring ? true : false);
    }
}
