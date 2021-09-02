using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUpgradingTrigger : ConditionTrigger
{
    [SerializeField] UpgradeZone upgradeZone;


    private void Awake()
    {
        upgradeZone.OnEndUpgrading += UpgradeZone_OnEndUpgrading;
    }

    private void UpgradeZone_OnEndUpgrading()
    {
        InvokeEvent();
    }
}
