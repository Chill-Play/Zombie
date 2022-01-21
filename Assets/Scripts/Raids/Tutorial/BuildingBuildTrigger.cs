using System;
using UnityEngine;

public class BuildingBuildTrigger : ConditionTrigger
{
    [SerializeField] private Construction construction;

    private void Awake()
    {
        construction.OnBuild += InvokeEvent;
    }
}