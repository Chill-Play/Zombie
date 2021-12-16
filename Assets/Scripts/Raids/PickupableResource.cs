using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickupableResource : MonoBehaviour
{
    public event System.Action OnPickup;

    [SerializeField] Resource resource;
    [SerializeField] float resourcesVelocity = 1;

    bool pickuped = false;
    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.GetComponent<PlayerResources>() != null && !pickuped)
        {
            pickuped = true;
            body.useGravity = true;
            body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
            body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
            resource.PickUp(other.transform);
            OnPickup?.Invoke();
        }
    }

}
