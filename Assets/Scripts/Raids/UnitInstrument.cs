using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    Tween rotationTween;

    protected override void Awake()
    {
        base.Awake();
        noiseController = NoiseController.Instance;
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
                for (int i = 0; i < useSpots.Length; i++)
                {
                    if (useSpots[i] != null && CanUse(useSpots[i]))
                    {
                        Use(useSpots[i]);          
                        noiseController.AddNoiseLevel(noisePerUse);
                        nextUse = Time.time + useRate;
                        break;
                    }
                }               
            }
        }
        else
        {   
            unitAnimation.SetInteraction(interactionType, false);
            unitInventory.ResetActiveItem();
        }
    }

    protected virtual void Use(Collider useSpot)
    {
        
    }

    protected virtual bool CanUse(Collider useSpot)
    {
        return true;
    }

    protected virtual void OnDisable()
    { 
        unitAnimation.SetInteraction(interactionType, false);
        unitInventory.ResetActiveItem();
    }

}
