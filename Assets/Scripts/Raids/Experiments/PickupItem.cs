using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickupItem : MonoBehaviour
{
    [SerializeField] SubjectId sectionId;
    [SerializeField] ResourceType resourceType;
    [SerializeField] float resourcesVelocity = 1f;
    [SerializeField] Vector3 resourceSpawnOffset = new Vector3(0f, 1f, 0f);

    bool pickuped = false;

    public SubjectId SectionId => sectionId;

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    void Pickup(GameObject user)
    {
        if (!pickuped)
        {
            Resource instance = Instantiate(resourceType.defaultPrefab, transform.position + resourceSpawnOffset, transform.rotation);
            instance.PickUp(user.transform);
            Rigidbody body = instance.GetComponent<Rigidbody>();
            body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
            body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerResources playerResources = other.GetComponent<PlayerResources>();
        if (playerResources != null)
        {
            Pickup(playerResources.gameObject);
            Destroy(gameObject);
        }
    }
}
