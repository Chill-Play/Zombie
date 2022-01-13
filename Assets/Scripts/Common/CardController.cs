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

[System.Serializable]
public class CardStatsSlot
{
    public Card card;
    public Dictionary<StatsType, int> statsInfo;

    public CardStatsSlot(Card card, Dictionary<StatsType, int> statsInfo)
    {
        this.card = card;
        this.statsInfo = statsInfo;
    }
}

[System.Serializable]
public class CardsStatsInfo
{
    public List<CardStatsSlot> cardSlots = new List<CardStatsSlot>();

    public CardsStatsInfo(List<Card> cardVariants)
    {
        foreach (var cardVariant in cardVariants)
        {
            Dictionary<StatsType, int> statsInfo = new Dictionary<StatsType, int>();
            foreach (var stat in cardVariant.CardStatsSettings)
            {
                statsInfo.Add(stat.statsType, 0);
            }
            cardSlots.Add(new CardStatsSlot(cardVariant, statsInfo));
        }
    }

    public CardStatsSlot GetCardStats(Card card)
    {        
        foreach (var slots in cardSlots)
        {
            if (slots.card == card)
            {
                return slots;
            }
        }
        return null;
    }
}

public class CardController : MonoBehaviour
{
    [SerializeField, CardSerialize] protected CardsInfo deckCards;
    [SerializeField, CardSerialize] protected CardsInfo activeCards = new CardsInfo();
    [SerializeField] int maxActiveSlots = 4;
    [SerializeField] List<Card> cardVariants = new List<Card>();
    [SerializeField] StatsType dStatsType;
    [HideInInspector, CardSerialize] public CardsStatsInfo cardsStatsInfo;

    public CardsInfo DeckCards => deckCards;
    public CardsInfo ActiveCards => activeCards;
    public List<Card> CardVariants => cardVariants;
    public CardStatsSlot CardStats(Card card) => cardsStatsInfo.GetCardStats(card);


    ResourcesController resourcesController;

    private void Awake()
    {      
        Load();
        if (cardsStatsInfo == null)        
            cardsStatsInfo = new CardsStatsInfo(cardVariants);        
        resourcesController = FindObjectOfType<ResourcesController>();      
    }

    public bool TryToActivateCard(CardSlot cardSlot)
    {
        if (activeCards.cardSlots.Count < maxActiveSlots)
        {
            activeCards.cardSlots.Add(cardSlot);
            if (deckCards.cardSlots.Contains(cardSlot))
            {
                deckCards.cardSlots.Remove(cardSlot);
            }
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

    public void UpgradeCardStats(Card card, StatsType statType, bool free = false, int value = 1)
    {        
        CardStatsSlot cardStats = CardStats(card);
        if (cardStats.statsInfo.ContainsKey(statType))
        {
            if (!free)
            {
                resourcesController.ResourcesCount.Subtract(statType.GetLevelCost(cardStats.statsInfo[statType]));
                resourcesController.UpdateResources();
            }
            cardStats.statsInfo[statType] += value;
            Save();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeCardStats(activeCards.cardSlots[0].card, dStatsType, true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(CardStats(activeCards.cardSlots[0].card).statsInfo[dStatsType]);
        }
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
