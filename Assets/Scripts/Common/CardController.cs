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

}
