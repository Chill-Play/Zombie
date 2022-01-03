using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickupableResource : MonoBehaviour
{
    public event System.Action OnPickup;

    [SerializeField] Sprite icon;
    [SerializeField] Resource resource;
    [SerializeField] float resourcesVelocity = 1;

    bool pickuped = false;
    Rigidbody body;
    UINumbers uiNumbers;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        uiNumbers = FindObjectOfType<UINumbers>();
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.GetComponent<PlayerBackpack>() != null && !pickuped)
        {
            Pickup(other.transform);
        }
    }

    public void Pickup(Transform picker)
    {
        pickuped = true;
        body.useGravity = true;
        body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
        body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
        resource.Pickup(picker);
        uiNumbers.SpawnNumber(transform.position, "+1", Vector2.zero, 15f, 10f, 1f, icon);
        OnPickup?.Invoke();
    }

}
