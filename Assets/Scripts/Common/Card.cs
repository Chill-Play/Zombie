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
    [SerializeField]  GameObject unitPrefab;

    public Sprite Icon => icon;
    public string CardName => cardName;
    public GameObject UnitVisual => unitVisual;
    public GameObject UnitPrefab => unitPrefab;

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
