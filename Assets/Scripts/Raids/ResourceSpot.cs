using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpot : MonoBehaviour
{
    public const int COUNT_PER_USE = 1;

    public event System.Action<ResourceSpot> OnSpotUsed;
    [SerializeField] int maxUses = 3;
    [SerializeField] ResourceType resourceType;
    [SerializeField] float noisePerUse = 10f;
    [SerializeField] float resourcesVelocity = 1f;
    [SerializeField] Vector3 resourceSpawnOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] ResourceInteractionType interactionType;

    float scale = 0;
    public ResourceInteractionType InteractionType => interactionType;

    int uses = 0;

    private void Awake()
    {
        scale = transform.localScale.magnitude;
        //Level.Instance.RegisterResourceSpot(this);
    }

    public void UseSpot(GameObject user)
    {
        if (uses >= maxUses)
        {
            return;
        }
        SpawnResources(user);
        uses++;

        Level.Instance.AddNoiseLevel(noisePerUse);

        transform.DOKill(true);
        transform.DOPunchScale(Vector3.one * 0.1f * scale, 0.2f, 3);

        if (uses >= maxUses)
        {
            OnSpotUsed?.Invoke(this);
            transform.DOScale(0f, 0.25f).SetEase(Ease.InCirc).OnComplete(() => Destroy(gameObject));
        }
    }

    void SpawnResources(GameObject user)
    {
        for (int i = 0; i < COUNT_PER_USE; i++)
        {
            Resource instance = Instantiate(resourceType.defaultPrefab, transform.position + resourceSpawnOffset, transform.rotation);
            instance.PickUp(user.transform);
            Rigidbody body = instance.GetComponent<Rigidbody>();
            body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
            body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
        }
    }
}
