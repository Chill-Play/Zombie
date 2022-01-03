using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Card")]
public class Card : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] protected string id;
    [SerializeField] Sprite icon;
    [SerializeField] string cardName;
    [SerializeField] GameObject unitVisual;
    [SerializeField]  GameObject campaignUnitPrefab;
    [SerializeField] GameObject raidUnitPrefab;

    public Sprite Icon => icon;
    public string CardName => cardName;
    public GameObject UnitVisual => unitVisual;
    public GameObject CampaignUnitPrefab => campaignUnitPrefab;
    public GameObject RaidUnitPrefab => raidUnitPrefab;

    public string Id => id;


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
