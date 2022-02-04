using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerResources : UnitInteracting
{
    [SerializeField] float useSpotRadius = 2f;
    [SerializeField] float useRate = 0.5f;
    [SerializeField] LayerMask resourceSpotsMask;
    [SerializeField] GameObject axeModel;
    [SerializeField] GameObject weaponModel;
    [SerializeField] bool inCamp = false;

    Collider[] resourceSpots = new Collider[1];
    float nextUse;
    bool interacting;
    UnitAnimation animation;
    SquadBackpack squadBackpack;

    public float UseRate { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        UseRate += useRate;
        animation = GetComponent<UnitAnimation>();
        if (axeModel.activeSelf)
        {
            axeModel.SetActive(false);
            weaponModel.SetActive(true);
        }
        
        if(inCamp)
        {
            CanMoveToResources = false;
        }
        squadBackpack = SquadBackpack.Instance;
    }


    public void AddUseRate(float useRate)
    {
        UseRate += useRate;
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(!inCamp && squadBackpack.IsFilled)
        {
            return;
        }
        if (inCamp && unitMovement.InputActive)
        {
            if (interacting)
            {
                if (axeModel.activeSelf)
                {
                    axeModel.SetActive(false);
                    weaponModel.SetActive(true);
                }
                interacting = false;
                animation.ResetInteraction();
            }
            return;
        }
        bool atTarget = false;
        if ((target.transform!= null && Vector3.Distance(transform.position, target.transform.position) < 0.5f) || !CanMoveToResources || atTarget)
        {
            if (CanMoveToResources)
            {
                //transform.rotation = target.transform.rotation;
            }
            atTarget = true;
        }

        int count = Physics.OverlapSphereNonAlloc(transform.position, useSpotRadius, resourceSpots, resourceSpotsMask);     
        if (count > 0 && atTarget)
        {
            if (!axeModel.activeSelf)
            {
                axeModel.SetActive(true);
                weaponModel.SetActive(false);
            }
            interacting = true;
            ResourceSpot spot = resourceSpots[0].GetComponent<ResourceSpot>();
            animation.SetInteraction(spot.InteractionType, true);
            if (nextUse < Time.time)
            {
                if (!inCamp)
                {
                    squadBackpack.UseSpot(ResourceSpot.COUNT_PER_USE);
                }
                spot.UseSpot(gameObject);
                nextUse = Time.time + useRate;
            }
            if (inCamp || !squadBackpack.IsFilled)
            {
                return;
            }
        }

        if (interacting)
        {
            if (axeModel.activeSelf)
            {
                axeModel.SetActive(false);
                weaponModel.SetActive(true);
            }
            interacting = false;
            animation.ResetInteraction();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (axeModel!= null && axeModel.activeSelf)
        {
            axeModel.SetActive(false);
            weaponModel.SetActive(true);
        }
        interacting = false;
        if (animation != null)
        {
            animation.ResetInteraction();
        }
    }    

}
