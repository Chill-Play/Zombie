using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUpgradingTrigger : ConditionTrigger
{
    [SerializeField] UpgradeZone upgradeZone;
    [SerializeField] bool doOnce = false;

  

    private void Start()
    {
        upgradeZone.OnEndUpgrading += UpgradeZone_OnEndUpgrading;
    }

    private void UpgradeZone_OnEndUpgrading()
    {
        InvokeEvent();
        if (doOnce)
        {
            upgradeZone.OnEndUpgrading -= UpgradeZone_OnEndUpgrading;
        }
    }
}
