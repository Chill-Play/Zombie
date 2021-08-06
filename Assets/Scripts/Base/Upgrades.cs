using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SimpleJSON;
using System;

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

    public event Action<ISaveableMapData> OnSave;

    public string SaveId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    private void Awake()
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
            result.needToResetSpeed = UpdateStatsLevel();
            SaveStatProgress();
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


    void SaveStatProgress()
    {
        OnSave?.Invoke(this);
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
        statValue = StatsManager.Instance.AddStat(statType);
        statUpgradeBar.UpdateValue(statValue);
        for (int i = 0; i < cost.Count; i++)
        {
            resources[cost[i].type] = cost[i].count;
            resourceBars[cost[i].type].Setup(cost[i].type, cost[i].count, cost[i].count);
        }
    }

    public JSONNode GetSaveData()
    {
        JSONObject result = new JSONObject();
        JSONObject jsonObject = new JSONObject();       
        for (int i = 0; i < cost.Count; i++)
        {
            jsonObject.Add(cost[i].type.ToString(), resources[cost[i].type]);
        }        
        result.Add("upgrades", jsonObject);
        return result;
    }

    public void Load(JSONNode loadData)
    {
        if (loadData.HasKey("save_data") && loadData["save_data"].HasKey("upgrades"))
        {            
            JSONNode upgradesNode = loadData["save_data"]["upgrades"];
            for (int i = 0; i < cost.Count; i++)
            {
                if (upgradesNode.HasKey(cost[i].type.ToString()))
                {
                    resources[cost[i].type] = upgradesNode[cost[i].type.ToString()].AsInt;
                }
            }
        }
        UpdateStatsLevel();
    }
}
