using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] float useSpotRadius = 2f;
    [SerializeField] float useRate = 0.5f;
    [SerializeField] LayerMask resourceSpotsMask;
    Collider[] resourceSpots = new Collider[1];
    float nextUse;
    bool interacting;
    //PlayerAnimation animation;

    public bool CanDigResources { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        //animation = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (unitMovement.InputActive || !CanDigResources)
        {
            if (interacting)
            {
                interacting = false;
                //animation.ResetInteraction();
            }
            return;
        }
        int count = Physics.OverlapSphereNonAlloc(transform.position, useSpotRadius, resourceSpots, resourceSpotsMask);
        if (count > 0)
        {
            interacting = true;
            ResourceSpot spot = resourceSpots[0].GetComponent<ResourceSpot>();
            //animation.SetInteraction(spot.InteractionType, true);
            if (nextUse < Time.time)
            {
                spot.UseSpot(gameObject);
                nextUse = Time.time + useRate;
            }
        }
        else
        {
            if (interacting)
            {
                interacting = false;
                //animation.ResetInteraction();
            }
        }
    }
}
