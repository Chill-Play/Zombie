using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgroActivator : MonoBehaviour
{
    [SerializeReference] float agroSpeed;

    Construction construction;

    public void GoAgro()
    {       
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = agroSpeed;
    }

    public void SubscribeToBarricade(Construction construction)
    {
        this.construction = construction;
        construction.OnBuild += GoAgro;
        GetComponent<UnitHealth>().OnDead += AgroActivator_OnDead;
    }

    private void AgroActivator_OnDead(EventMessage<Empty> obj)
    {
        if (construction != null)
        {
            construction.OnBuild -= GoAgro;
        }
    }
}

