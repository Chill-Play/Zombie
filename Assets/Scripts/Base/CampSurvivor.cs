using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSurvivor : MonoBehaviour
{
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] ResourceType resourceType;
    [SerializeField] private Animator anim;
    [SerializeField] private int danceAnimationCount;
    
    HQBuilding hq;
    private Vector3 destination;
    private bool isForcedStop = false;

    public event System.Action<CampSurvivor> OnReachBuilding;

    private void Awake()
    {
        hq = FindObjectOfType<HQBuilding>();
    }

    void Update()
    {
        if (unitMovement.IsReachDestination && !isForcedStop)
        {
            hq.AddPoint(1);
            OnReachBuilding?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void ForceToStop()
    {
        isForcedStop = true;
        unitMovement.StopMoving();
        anim.SetBool("Dance", true);
        anim.SetInteger("DanceId", Random.Range(0, danceAnimationCount));
    }

    public void ForceToMove()
    {
        isForcedStop = false;
        unitMovement.MoveTo(destination);
        anim.SetBool("Dance", false);
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }
}
