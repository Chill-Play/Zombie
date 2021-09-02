using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorPickUpTrigger : ConditionTrigger
{
    [SerializeField] List<SurvivorPickup> survivorPickups = new List<SurvivorPickup>();

    int survivorCount = 0;

    private void Awake()
    {
        for (int i = 0; i < survivorPickups.Count; i++)
        {
            survivorPickups[i].OnPickup += SurvivorPickUpTrigger_OnPickup;
        }
        survivorCount = survivorPickups.Count;
    }

    private void SurvivorPickUpTrigger_OnPickup(SurvivorPickup obj)
    {
        survivorCount--;
        if (survivorCount == 0)
        {
            InvokeEvent();
            survivorPickups.Clear();
        }
    }
}
