using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInstrument : UnitActivity
{
    [SerializeField] float useRate = 1f;
    [SerializeField] float noisePerUse = 10f;
    [SerializeField] GameObject instrument;
    [SerializeField] ResourceInteractionType interactionType;
    [SerializeField] UnitAnimation unitAnimation;

    float nextUse;
    NoiseController noiseController;
    UnitMovement unitMovement;
    UnitInventory unitInventory;

    protected override void Awake()
    {
        base.Awake();
        noiseController = FindObjectOfType<NoiseController>();
        unitMovement = GetComponent<UnitMovement>();
        unitInventory = GetComponent<UnitInventory>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (count > 0 && (unitMovement == null || !unitMovement.InputActive))
        {
            unitAnimation.SetInteraction(interactionType, true);
            unitInventory.SetActiveItem(instrument);
            if (nextUse < Time.time)
            {
                if (Use())
                {

                    noiseController.AddNoiseLevel(noisePerUse);
                    nextUse = Time.time + useRate;
                }
            }
        }
        else
        {
            unitAnimation.SetInteraction(interactionType, false);
            unitInventory.ResetActiveItem();
        }
    }

    protected virtual bool Use()
    {
        return false;
    }

    protected virtual void OnDisable()
    {
        unitAnimation.SetInteraction(interactionType, false);
        unitInventory.ResetActiveItem();
    }

}
