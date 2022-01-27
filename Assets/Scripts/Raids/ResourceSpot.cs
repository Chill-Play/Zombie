using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSpot : MonoBehaviour
{
    public const int COUNT_PER_USE = 1;

    public event System.Action<ResourceSpot> OnSpotUsed;
    [SerializeField] int maxUses = 3;
    [SerializeField] ResourceType resourceType;
    [SerializeField] List<ResourceType> possibleResourceTypes = new List<ResourceType>();
    [SerializeField] float noisePerUse = 10f;
    [SerializeField] float resourcesVelocity = 1f;
    [SerializeField] Vector3 resourceSpawnOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] ResourceInteractionType interactionType;
    
    int uses = 0;
    float scale = 0;
    IEnumerable<INoiseListener> noiseListeners;
    UINumbers uiNumbers;

    public ResourceInteractionType InteractionType => interactionType;

   

    private void Awake()
    {
        scale = transform.localScale.magnitude;
        noiseListeners = FindObjectsOfType<MonoBehaviour>().OfType<INoiseListener>();  
        uiNumbers = UINumbers.Instance;
    }

    private void Start()
    {
        ResourcesController resourcesController = ResourcesController.Instance;
        if (!resourcesController.OpenedResources.Contains(resourceType))
        {
            bool found = false;
            for (int i = 0; i < possibleResourceTypes.Count; i++)
            {
                if (resourcesController.OpenedResources.Contains(possibleResourceTypes[i]))
                {
                    resourceType = possibleResourceTypes[i];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                resourceType = resourcesController.DefaultResourceType;
            }
        }
    }

    public void UseSpot(GameObject user)
    {
        if (uses >= maxUses)
        {
            return;
        }
        SpawnResources(user);
        uses++;

        INoiseListener noiseListener = noiseListeners.FirstOrDefault();
        if (noiseListener != null)
        {
            noiseListener.AddNoiseLevel(noisePerUse);      
        }

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
            instance.Pickup(user.transform);
            Rigidbody body = instance.GetComponent<Rigidbody>();
            body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
            body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;        
            UINumber number = uiNumbers.GetNumber(transform.position + Vector3.up * 2f, "+" + COUNT_PER_USE, Vector2.zero, 0f, 0f, true);
            uiNumbers.AttachImage(number, resourceType.icon);
            uiNumbers.MoveUpNumber(number, 60f, 0.8f, () => uiNumbers.End(number));
        }
    }
}
