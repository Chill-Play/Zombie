using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatsUpgrader : MonoBehaviour
{
    [SerializeField] StatsType damageStat;
    [SerializeField] StatsType attackSpeedStat;
    [SerializeField] StatsType healthStat;

    [SerializeField] UnitHealth health;
    [SerializeField] UnitShooting shooting;

    [SerializeField] float damagePerLevel;
    [SerializeField] float healthPerLevel;
    [SerializeField] float attackRatePerLevel;

    // Start is called before the first frame update
    void Start()
    {
        var manager = FindObjectOfType<StatsManager>();
        health.AddMaxHealth(healthPerLevel * manager.GetStatInfo(healthStat).level);
        shooting.AddDamage(damagePerLevel * manager.GetStatInfo(damageStat).level);
        shooting.AddAttackRate(attackRatePerLevel * manager.GetStatInfo(attackSpeedStat).level);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
