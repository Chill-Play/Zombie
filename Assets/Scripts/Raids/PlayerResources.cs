using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] InteractivePointDetection interactivePointDetection;
    [SerializeField] float useSpotRadius = 2f;
    [SerializeField] float useRate = 0.5f;
    [SerializeField] LayerMask resourceSpotsMask;
    [SerializeField] GameObject axeModel;
    [SerializeField] GameObject weaponModel;
    Collider[] resourceSpots = new Collider[1];
    float nextUse;
    bool interacting;
    UnitAnimation animation;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<UnitAnimation>();
        if (axeModel.activeSelf)
        {
            axeModel.SetActive(false);
            weaponModel.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, useSpotRadius, resourceSpots, resourceSpotsMask);
        if (count > 0)
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
                spot.UseSpot(gameObject);
                nextUse = Time.time + useRate;
            }
            return;
        }
        if (!interacting)
        {
            if (interactivePointDetection.Target != null)
            {              
                unitMovement.MoveTo(interactivePointDetection.Target.transform.position);
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

    private void OnDisable()
    {       
        if (axeModel.activeSelf)
        {
            axeModel.SetActive(false);
            weaponModel.SetActive(true);
        }
        interacting = false;
        animation.ResetInteraction();
        unitMovement.StopMoving();
    }
}
