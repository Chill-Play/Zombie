using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Card")]
public class Card : ScriptableObject, ISerializationCallbackReceiver
{
    [System.Serializable]
    public class CardStats
    {
        public StatsType statsType;
        public float baseStatValue;
        public float statsCountPreLevel;
    }

    [SerializeField] protected string id;
    [SerializeField] Sprite icon;
    [SerializeField] Sprite newSpecialistIcon;
    [SerializeField] Sprite hideNewSpecialistIcon;
    [SerializeField] string cardName;
    [SerializeField] GameObject unitVisual;
    [SerializeField] GameObject campaignUnitPrefab;
    [SerializeField] GameObject raidUnitPrefab;
    [SerializeField] List<CardStats> cardStats = new List<CardStats>();

    public Sprite Icon => icon;
    public Sprite NewSpecialistIcon => newSpecialistIcon;
    public Sprite HideNewSpecialistIcon => hideNewSpecialistIcon;
    public string CardName => cardName;
    public GameObject UnitVisual => unitVisual;
    public GameObject CampaignUnitPrefab => campaignUnitPrefab;
    public GameObject RaidUnitPrefab => raidUnitPrefab;
    public List<CardStats> CardStatsSettings => cardStats;

    public string Id => id;

    public float GetStatValue(StatsType statType, int level)
    {
       foreach(var stat in cardStats)
       {
         if(stat.statsType == statType)
         {
           return level * stat.statsCountPreLevel;
         }
       }
       return 0f;
    }

    public float GetStatValueDisplay(StatsType statType, int level)
    {
        foreach (var stat in cardStats)
        {
            if (stat.statsType == statType)
            {
                return stat.baseStatValue + level * stat.statsCountPreLevel;
            }
        }
        return 0f;
    }

    public void OnAfterDeserialize()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
