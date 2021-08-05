using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SimpleJSON;

public class Upgrades : MonoBehaviour, IBuilding, ISaveableMapData
{
    [System.Serializable]
    public struct CostInfo
    {
        public ResourceType type;
        public int count;
    }   

    [SerializeField] public StatsType statType;  

    [SerializeField] List<CostInfo> cost = new List<CostInfo>();

    [SerializeField] Transform resourcesLayout;
    [SerializeField] StatUpgradeBar statUpgradeBar;
    [SerializeField] ResourceBarWhithMaxCount resourceBarPrefab;

    int statValue;

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    Dictionary<ResourceType, ResourceBarWhithMaxCount> resourceBars = new Dictionary<ResourceType, ResourceBarWhithMaxCount>();

    public string SaveId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    private void OnEnable()
    {
        statValue = StatsManager.Instance.StatsInfo[statType];
        statUpgradeBar.SetupBar(statType, statValue);

        for (int i = 0; i < cost.Count; i++)
        {
            resources.Add(cost[i].type, cost[i].count);
            ResourceBarWhithMaxCount bar = Instantiate(resourceBarPrefab, resourcesLayout);
            bar.Setup(cost[i].type, cost[i].count, cost[i].count);
            resourceBars.Add(cost[i].type, bar);
        }
    }

    public BuildingReport TryUseResources(List<ResourceType> playerResources, int count)
    {
        Debug.Log("AZZAZA");
        BuildingReport result = new BuildingReport();
        for (int i = 0; i < playerResources.Count; i++)
        {
            if (resources.TryGetValue(playerResources[i], out int needCount))
            {
                ResourceType type = playerResources[i];
                int playerCount = type.Count;
                if (playerCount > 0)
                {
                    int useCount = Mathf.Min(count, playerCount);
                    useCount = Mathf.Min(useCount, needCount);
                    type.Count -= useCount;
                    resources[playerResources[i]] -= useCount;
                    result.resourcesUsed = true;
                    if (useCount > 0)
                    {
                        CreateResourceAnimation(playerResources[i], i);
                    }
                }
            }
        }
        if (result.resourcesUsed)
        {
            ResourcesController.Instance.UpdateResources();
            result.buildingFinished = UpdateStatsLevel();
            SaveBuilding();
        }

        return result;
    }

    void CreateResourceAnimation(ResourceType type, int i)
    {
        Transform playerTransform = CampGameplayController.Instance.playerInstance;
        Resource instance = Instantiate(type.defaultPrefab, playerTransform.position, Quaternion.LookRotation(UnityEngine.Random.insideUnitSphere));
        instance.GetComponent<Rigidbody>().isKinematic = true;
        instance.transform.DOJump(transform.position, 1f, 1, 0.3f).OnComplete(() => Destroy(instance)).SetDelay((float)i * 0.1f);
    }


    void SaveBuilding()
    {
       // MapController.Instance.Save(this);
    }


    bool UpdateStatsLevel()
    {
        bool result = false;
        int finishedResources = 0;
        foreach (var pair in resources)
        {
            resourceBars[pair.Key].UpdateValue(pair.Value);
            if (pair.Value == 0)
            {               
                finishedResources++;
            }
        }
        if (finishedResources == resources.Count)
        {
            result = true;
            AddStat();
        }     
        return result;
    }

    void AddStat()
    {
        
    }

    public JSONNode GetSaveData()
    {
        throw new System.NotImplementedException();
    }

    public void Load(JSONNode loadData)
    {
        throw new System.NotImplementedException();
    }
}
