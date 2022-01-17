using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Upgradable : BaseObject, IBuildable
{
    public event System.Action OnUpgradeRangeEnter;
    public event System.Action OnUpgradeRangeExit;

    public event System.Action OnUpdateResourcesSpend;
    public event System.Action OnLevelUp;

    [SerializeField] protected ResourcesInfo baseCost;
    [SerializeField] protected float costMultiplier;
    [SerializeField] protected float costPower;
    [SerializeField] Transform uiPoint;
    [SerializeField] ColliderEventListener upgradeRangeCollider;
    [SerializeField] ColliderEventListener upgradeZoneCollider;

    [BaseSerialize] protected int level;
    [BaseSerialize] ResourcesInfo resourcesSpent = new ResourcesInfo();

    ResourcesInfo cost;
    PlayerBuilding player;
    bool inZone = false;
    bool needToExitZone = false;

    public ResourcesInfo ResourcesSpent => resourcesSpent;
    public ResourcesInfo Cost => cost;
    public int Level => level;
    public Transform UIPoint => uiPoint;
    public bool CanBuild => enabled && inZone && !needToExitZone;

    private void Awake()
    {
        upgradeRangeCollider.OnTriggerEnterEvent += UpgradeRangeCollider_OnTriggerEnterEvent;
        upgradeRangeCollider.OnTriggerExitEvent += UpgradeRangeCollider_OnTriggerExitEvent;
        upgradeZoneCollider.OnTriggerEnterEvent += UpgradeZoneCollider_OnTriggerEnterEvent; ;
        upgradeZoneCollider.OnTriggerExitEvent += UpgradeZoneCollider_OnTriggerExitEvent; ;
    }

    private void Start()
    {
        cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
        if (resourcesSpent.Slots.Count < cost.Slots.Count)
        {
            resourcesSpent.ApplyTypes(cost);
        }
    }

    public void SpendResources(ResourcesInfo info, int count)
    {
        resourcesSpent.Spend(info, cost, count, CreateResourceAnimation);
        if (cost.IsFilled(resourcesSpent))
        {
            level++;
            resourcesSpent.EmptySlots();
            cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
            needToExitZone = true;
            OnLevelUp?.Invoke();
        }
        OnUpdateResourcesSpend?.Invoke();
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
            instance.transform.DOJump(transform.position, 1f, 1, 0.3f).OnComplete(() => Destroy(instance.gameObject)).SetDelay((float)order * 0.1f);
        }
    }

    private void UpgradeRangeCollider_OnTriggerEnterEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            OnUpgradeRangeEnter?.Invoke();
        }
    }

    private void UpgradeRangeCollider_OnTriggerExitEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            OnUpgradeRangeExit?.Invoke();
        }
    }

    private void UpgradeZoneCollider_OnTriggerEnterEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            inZone = true;
        }
    }

    private void UpgradeZoneCollider_OnTriggerExitEvent(Collider obj)
    {
        if (obj.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            needToExitZone = false;
            inZone = false;
        }
    }
}
