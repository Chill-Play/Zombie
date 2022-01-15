using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Upgradable : BaseObject, IBuildable
{
    [SerializeField] protected ResourcesInfo baseCost;
    [SerializeField] protected float costMultiplier;
    [SerializeField] protected float costPower;

    [BaseSerialize] protected int level;
    [BaseSerialize] ResourcesInfo resourcesSpent = new ResourcesInfo();

    ResourcesInfo cost;
    PlayerBuilding player;

    public bool CanBuild => enabled;

    private void Start()
    {
        cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
    }

    public void SpendResources(ResourcesInfo info, int count)
    {
        resourcesSpent.Spend(info, cost, count, CreateResourceAnimation);
        if (cost.IsFilled(resourcesSpent))
        {
            level++;
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
}
