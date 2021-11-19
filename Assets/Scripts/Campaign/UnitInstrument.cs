using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInstrument : UnitActivity
{
    [SerializeField] float useRate = 1f;
    [SerializeField] float noisePerUse = 10f;
    [SerializeField] bool debug = false;

    float nextUse;
    NoiseController noiseController;
    UnitMovement unitMovement;

    protected override void Awake()
    {
        base.Awake();
        noiseController = FindObjectOfType<NoiseController>();
        unitMovement = GetComponent<UnitMovement>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (debug)
        {
            Debug.Log("count " + count.ToString() + " unitMovement.VelocityActive " + unitMovement.VelocityActive.ToString());
        }
        if (count > 0 && (unitMovement == null || !unitMovement.InputActive))
        {
            if (nextUse < Time.time)
            {
                if (Use())
                {
                    noiseController.AddNoiseLevel(noisePerUse);
                    nextUse = Time.time + useRate;
                }
            }
        }
    }

    protected virtual bool Use()
    {
        return false;
    }

}
