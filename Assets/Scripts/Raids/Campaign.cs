using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign : SingletonMono<Campaign>
{
    [SerializeField] List<Unit> units = new List<Unit>();
    [SerializeField] List<CardSlot> rewardCards = new List<CardSlot>();

    public int SpecialistCount => units.Count;
    public List<CardSlot> RewardCards => rewardCards;

    private void Awake()
    {
        if (rewardCards.Count > 0)
        {
            CardController.Instance.TryToActivateCard(rewardCards[0]);
        }
    }

}
