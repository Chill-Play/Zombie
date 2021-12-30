using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSlot
{
    public Card card;
    public int level;    
}

public class CardController : MonoBehaviour
{
    [SerializeField] List<CardSlot> cardSlots = new List<CardSlot>();
    [SerializeField] List<CardSlot> activeCardSlots = new List<CardSlot>();
    [SerializeField] int maxActiveSlots = 4;

    public List<CardSlot> CardSlots => cardSlots;
    public List<CardSlot> ActiveCardSlots => activeCardSlots;

    public bool TryToActivateCard(CardSlot cardSlot)
    {
        if (activeCardSlots.Count < maxActiveSlots)
        {
            activeCardSlots.Add(cardSlot);
            cardSlots.Remove(cardSlot);
            return true;
        }

        return false;
    }

    public void DeactivateCard(CardSlot cardSlot)
    {
        cardSlots.Add(cardSlot);
        activeCardSlots.Remove(cardSlot);
    }

    public bool CanUpgrade(CardSlot cardSlot, int requireCards = 1)
    {
        int currentCount = 0;
        if (CanUpgradeInSlots(cardSlot, cardSlots, ref currentCount, requireCards) ||
            CanUpgradeInSlots(cardSlot, activeCardSlots, ref currentCount, requireCards))
        {
            return true;
        } 
        return false;
    }

    bool CanUpgradeInSlots(CardSlot cardSlot, List<CardSlot> checkingCardSlots,ref int currentCount, int requireCards)
    {
        for (int i = 0; i < checkingCardSlots.Count; i++)
        {
            if (cardSlot != checkingCardSlots[i] && cardSlot.card == checkingCardSlots[i].card && cardSlot.level == checkingCardSlots[i].level)
            {
                currentCount++;
                if (currentCount >= requireCards)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void UpgradeCard(CardSlot cardSlot, int requireCards = 1)
    {
        int currentCount = 0;        
        if (UpgradeCardInSlots(cardSlot, cardSlots, ref currentCount, requireCards) ||
            UpgradeCardInSlots(cardSlot, activeCardSlots, ref currentCount, requireCards))
        {
            cardSlot.level++;
        }       
    }

    bool UpgradeCardInSlots(CardSlot cardSlot, List<CardSlot> checkingCardSlots, ref int currentCount, int requireCards)
    {
        for (int i = checkingCardSlots.Count - 1; i >= 0; i--)
        {
            if (cardSlot != checkingCardSlots[i] && cardSlot.card == checkingCardSlots[i].card && cardSlot.level == checkingCardSlots[i].level)
            {
                currentCount++;
                checkingCardSlots.RemoveAt(i);                
                if (currentCount >= requireCards)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
