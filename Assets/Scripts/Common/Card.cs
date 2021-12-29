using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZombieSim/Card")]
public class Card : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] string cardName;
    [SerializeField] GameObject unitVisual;
    [SerializeField]  GameObject unitPrefab;

    public Sprite Icon => icon;
    public string CardName => cardName;
    public GameObject UnitVisual => unitVisual;


}
