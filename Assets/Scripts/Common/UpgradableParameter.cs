using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradableParameter
{
    [SerializeField] float baseValue;
    [SerializeField] float levelModifier;

    float value;
    public float Value => value;
    


    public void Initialize(int level)
    {
        value = baseValue + (levelModifier * level);
    }
}
