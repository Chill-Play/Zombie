using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] float useSpotRadius = 2f;
    [SerializeField] float useRate = 0.5f;
    [SerializeField] LayerMask resourceSpotsMask;
    [SerializeField] GameObject axeModel;
    [SerializeField] GameObject weaponModel;
    Collider[] resourceSpots = new Collider[1];
    float nextUse;
    bool interacting;
    UnitAnimation animation;

    public bool IsSquadStoped { get; set; } = true;
    public bool CanDigResources { get; set; } = true;

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
        if (CanDigResources && IsSquadStoped)
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
}
