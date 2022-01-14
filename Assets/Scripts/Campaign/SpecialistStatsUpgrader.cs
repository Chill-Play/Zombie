using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialistStatsUpgrader : MonoBehaviour
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

    public void UpdateStats(Card card)
    {
        CardController cardController = FindObjectOfType<CardController>();
        CardStatsSlot cardStatsSlot = cardController.CardStats(card);
        health.AddMaxHealth(healthPerLevel * cardStatsSlot.statsInfo[healthStat]);
        shooting.AddDamage(damagePerLevel * cardStatsSlot.statsInfo[damageStat]);
        if (resources != null)
        {
            resources.AddUseRate(interactingSpeedPerLevel * cardStatsSlot.statsInfo[interactingSpeed]);
        }
        if (unitConstructing != null)
        {
            unitConstructing.AddConstructingPower(interactingSpeedPerLevel * cardStatsSlot.statsInfo[interactingSpeed]);
        }
        if (unitRepairing != null)
        {
            unitRepairing.AddRepairingPower(interactingSpeedPerLevel * cardStatsSlot.statsInfo[interactingSpeed]);
        }
    }
}
