using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RewardStar : MonoBehaviour
{
    public event System.Action<RewardStar> OnStarCollected;

    [SerializeField] PickupableResource pickupableResource; 

    Rigidbody body;
    bool placed = false;
    Tween rotationTween;    

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        pickupableResource.enabled = false;
        pickupableResource.OnPickup += PickupableResource_OnPickup;
    }

    private void PickupableResource_OnPickup()
    {
        OnStarCollected?.Invoke(this);
        rotationTween.Kill(false);
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= 0f && !placed)
        {
            placed = true;
            body.useGravity = false;
            body.velocity = Vector3.zero;
            rotationTween = transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 3f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
            transform.position = transform.position.SetY(0f);
            pickupableResource.enabled = true;
        }        
    }

    public void PickupStar(Transform picker)
    {
        pickupableResource.Pickup(picker);
    }
}
