using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campaign : MonoBehaviour
{
    [SerializeField] List<Unit> units = new List<Unit>();
    [SerializeField] List<CardSlot> rewardCards = new List<CardSlot>();

    public int SpecialistCount => units.Count;
    public List<CardSlot> RewardCards => rewardCards;
    [SerializeField] private CardController cardController;

    void Start()
    {
        Squad squad = FindObjectOfType<Squad>();
        cardController.TryToActivateCard(rewardCards[0]);
        CardsInfo activeCards = FindObjectOfType<CardController>().ActiveCards;
        for (int i = 0; i < activeCards.cardSlots.Count; i++)
        {

            GameObject instance = Instantiate(activeCards.cardSlots[i].card.CampaignUnitPrefab, squad.Units[0].transform.position, squad.transform.rotation);
            squad.AddUnit(instance.GetComponent<Unit>());
        }        
    }

   
}
