using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialistUpgradeCounter : UpgradeCounter
{
    [SerializeField] StatsType statType;

    CardController cardController;

    protected override void Awake()
    {
        base.Awake();
        cardController = CardController.Instance;
        cardController.OnCardUpgraded += CardController_OnCardUpgraded;
    }

    private void CardController_OnCardUpgraded(Card card, StatsType statType)
    {
        if (this.statType == statType)
        {
            RequireUpdate();
        }
    }

    public override int AvailableUpgrades()
    {
        int result = base.AvailableUpgrades();

        foreach (var slot in cardController.ActiveCards.cardSlots)
        {
            int lvl = cardController.CardStats(slot.card).statsInfo[statType];
            ResourcesInfo cost = statType.GetLevelCost(lvl);
            if (cost.IsFilled(resourcesController.ResourcesCount))
            {
                result++;
            }
        }

        return result;
    }

}
