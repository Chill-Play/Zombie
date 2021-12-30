using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[System.Serializable]
public class CardsInfo
{
    public List<CardSlot> cardSlots = new List<CardSlot>();

    public void AddSlot(Card card, int level)
    {
        cardSlots.Add(new CardSlot(card, level));
    }

    public int Count => cardSlots.Count;
}

[System.Serializable]
public class CardSlot
{
    public CardSlot(Card card, int level)
    {
        this.card = card;
        this.level = level;
    }

    public Card card;
    public int level = 1;    
}

public class CardController : MonoBehaviour
{
    [SerializeField, CardSerialize] public CardsInfo deckCards;
    [CardSerialize] protected CardsInfo activeCards = new CardsInfo();
    [SerializeField] int maxActiveSlots = 4;
    [SerializeField] List<Card> cardVariants = new List<Card>();

    public CardsInfo DeckCards => deckCards;
    public CardsInfo ActiveCards => activeCards;

    private void Awake()
    {
        Load();
    }

    public bool TryToActivateCard(CardSlot cardSlot)
    {
        if (activeCards.cardSlots.Count < maxActiveSlots)
        {
            activeCards.cardSlots.Add(cardSlot);
            deckCards.cardSlots.Remove(cardSlot);
            Save();
            return true;
        }

        return false;
    }

    public void DeactivateCard(CardSlot cardSlot)
    {
        deckCards.cardSlots.Add(cardSlot);
        activeCards.cardSlots.Remove(cardSlot);
        Save();
    }

    public bool CanUpgrade(CardSlot cardSlot, int requireCards = 1)
    {
        int currentCount = 0;
        if (CanUpgradeInSlots(cardSlot, deckCards.cardSlots, ref currentCount, requireCards) ||
            CanUpgradeInSlots(cardSlot, activeCards.cardSlots, ref currentCount, requireCards))
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
        if (UpgradeCardInSlots(cardSlot, deckCards.cardSlots, ref currentCount, requireCards) ||
            UpgradeCardInSlots(cardSlot, activeCards.cardSlots, ref currentCount, requireCards))
        {
            cardSlot.level++;
            Save();
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

    public Card GetCardVariant(string id)
    {
        for (int i = 0; i < cardVariants.Count; i++)
        {
            if (cardVariants[i].Id == id)
            {
                return cardVariants[i];
            }
        }

        Debug.LogError("Card variant do not exist!");
        return null;
    }

    void Save()
    {
        var json = CardSerialization.SerializeCards(this);
        PlayerPrefs.SetString("CardsInfo", json);
    }

    void Load()
    {
        var json = PlayerPrefs.GetString("CardsInfo", null);
        CardSerialization.DeserializeCards(json, this);
    }
}
