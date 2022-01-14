using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsUpgrader : MonoBehaviour
{
    [SerializeField] StatsType damageStat;
    [SerializeField] StatsType healthStat;
    [SerializeField] StatsType interactingSpeed;

    [SerializeField] UnitHealth health;
    [SerializeField] UnitShooting shooting;
    [SerializeField] PlayerResources resources;
    [SerializeField] UnitConstructing unitConstructing;
    [SerializeField] UnitRepairing unitRepairing;

    [SerializeField] float damagePerLevel;
    [SerializeField] float healthPerLevel; 
    [SerializeField] float interactingSpeedPerLevel;

    // Start is called before the first frame update
    void Start()
    {
        var manager = FindObjectOfType<StatsManager>();
        health.AddMaxHealth(healthPerLevel * manager.GetStatInfo(healthStat).level);
        shooting.AddDamage(damagePerLevel * manager.GetStatInfo(damageStat).level);      
        if (resources != null)
        {
            resources.AddUseRate(interactingSpeedPerLevel * manager.GetStatInfo(interactingSpeed).level);
        }
        if (unitConstructing != null)
        {
            unitConstructing.AddConstructingPower(interactingSpeedPerLevel * manager.GetStatInfo(interactingSpeed).level);
        }
        if (unitRepairing != null)
        {
            unitRepairing.AddRepairingPower(interactingSpeedPerLevel * manager.GetStatInfo(interactingSpeed).level);
        }
    }
}
