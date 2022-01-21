using System;
using UnityEngine;

public class FenceBuildTrigger : ConditionTrigger
{
    [SerializeField] private Construction construction;
    [SerializeField] private Collider survivorCamp;

    private void Awake()
    {
        construction.OnBuild += ConstructionBuilded;
    }

    void ConstructionBuilded()
    {
        survivorCamp.enabled = true;
        InvokeEvent();
    }
}