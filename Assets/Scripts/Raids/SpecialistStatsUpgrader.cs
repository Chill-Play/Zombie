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
 

    public void UpdateStats(Card card)
    {
        CardController cardController = CardController.Instance;
        CardStatsSlot cardStatsSlot = cardController.CardStats(card);
        health.AddMaxHealth(card.GetStatValue(healthStat , cardStatsSlot.statsInfo[healthStat]));
        shooting.AddDamage(card.GetStatValue(damageStat, cardStatsSlot.statsInfo[damageStat]));
        if (resources != null)
        {
            resources.AddUseRate(card.GetStatValue(interactingSpeed, cardStatsSlot.statsInfo[interactingSpeed]));
        }
        if (unitConstructing != null)
        {
            unitConstructing.AddConstructingPower(card.GetStatValue(interactingSpeed, cardStatsSlot.statsInfo[interactingSpeed]));
        }
        if (unitRepairing != null)
        {
            unitRepairing.AddRepairingPower(card.GetStatValue(interactingSpeed, cardStatsSlot.statsInfo[interactingSpeed]));
        }
    }
}
