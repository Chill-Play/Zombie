using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Buildable : BaseObject
{
    public event System.Action OnEnabled;
    public event System.Action OnUpdate;
    public event System.Action<bool> OnBuilt; // true if built after deserialization 

    [SerializeField] ResourcesInfo cost;
    [BaseSerialize] ResourcesInfo resourcesSpent = new ResourcesInfo();
    [BaseSerialize] bool built;

    PlayerBuilding player;
    ZombiesLevelController zombiesLevelController;

    public ResourcesInfo Cost => cost;
    public ResourcesInfo ResourcesSpent => resourcesSpent;
    bool initialized;
    public bool Built => built;


    public void Awake()
    {
        zombiesLevelController = FindObjectOfType<ZombiesLevelController>();
        if (resourcesSpent.Slots.Count < cost.Slots.Count)
        {
            resourcesSpent.ApplyTypes(cost);
        }
    }


    void Start()
    {
        initialized = true;
        OnUpdate?.Invoke(); //Refactor
    }


    private void OnEnable()
    {
        //if (initialized)
        {
            OnEnabled?.Invoke();
        }
    }


    public void SpendResources(ResourcesInfo info, int count)
    {
        if (Built) return;
        resourcesSpent.Spend(info, cost, count, CreateResourceAnimation);        
        if (cost.IsFilled(resourcesSpent))
        {
            FinishBuilding(false);
        }
        else
        {
            OnUpdate?.Invoke();
        }
        RequireSave();
    }

    void CreateResourceAnimation(ResourceType type, int count, int order)
    {
        if (count > 0)
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerBuilding>();
            }
            Resource instance = Instantiate(type.defaultPrefab, player.transform.position, Quaternion.LookRotation(UnityEngine.Random.insideUnitSphere));
            instance.GetComponent<Rigidbody>().isKinematic = true;
            instance.transform.DOJump(transform.position, 1f, 1, 0.3f).OnComplete(() => Destroy(instance)).SetDelay((float)order * 0.1f);  
        }
     }
       

    private void FinishBuilding(bool afterDeserialization)
    {
        built = true;
        enabled = false;
        if(!afterDeserialization && zombiesLevelController.RaidIsPlayed > 1)
        {
            AdvertisementManager.Instance.TryShowInterstitial("base_finished_building");
        }
        OnBuilt?.Invoke(afterDeserialization);
    }

    public override void BaseAfterDeserialize()
    {
        base.BaseAfterDeserialize();
        if (built)
        {
            FinishBuilding(true);
        }
    }
}
