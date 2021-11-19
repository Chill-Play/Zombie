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
    [SerializeField] bool inCamp = false;

    Collider[] resourceSpots = new Collider[1];
    float nextUse;
    bool interacting;
    UnitAnimation animation;
    SquadBackpack squadBackpack;


    InteractivePoint.WorkingPoint target;


    public bool CanMoveToResources { get; set; } = true;
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
        if (!inCamp)
        {
            enabled = false;
        }
        else
        {
            CanMoveToResources = false;
        }
        squadBackpack = FindObjectOfType<SquadBackpack>();
    }


    public void AddUseRate(float useRate)
    {
        UseRate += useRate;
    }


    private void OnEnable()
    {
        TryToFindFreePoint();
        interactivePointDetection.OnTargetChanged += InteractivePointDetection_OnTargetChanged;
    }

    private void InteractivePointDetection_OnTargetChanged(InteractivePoint interactivePoint, InteractivePoint newInteractivePoint)
    {
        if (interactivePoint != null && target.index != -1)
        {
            interactivePoint.FreePoint(target);
        }
        target = new InteractivePoint.WorkingPoint(null, -1);
    }

    void TryToFindFreePoint()
    {
        if (CanMoveToResources && interactivePointDetection.Target!= null)
        {
            target = interactivePointDetection.Target.GetFreePoint(transform.position, unitMovement);

            if (target.transform != null && unitMovement.CanReachDestination(target.transform.position))
            {               
                interactivePointDetection.Target.TakePoint(target);
                unitMovement.MoveTo(target.transform.position);
            }
            else
            {
               // Debug.DrawLine(transform.position + Vector3.up, target.transform.position, Color.yellow, 10f);
                interactivePointDetection.NullTarget();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                transform.rotation = target.transform.rotation;
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

        if (target.transform == null)
        {
            TryToFindFreePoint();
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
        interactivePointDetection.OnTargetChanged -= InteractivePointDetection_OnTargetChanged; 
        if (target.index != -1)
        {
            if (interactivePointDetection.Target != null)
            {
                interactivePointDetection.Target.FreePoint(target);
            }
            target = new InteractivePoint.WorkingPoint(null, -1);           
        }
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
