using DG.Tweening;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : BaseBuilding
{
    public event System.Action OnBuildingInited;
    public event System.Action OnDataLoaded;
    public event System.Action OnUpdate;

    [SerializeField] List<CostInfo> cost = new List<CostInfo>();
    [SerializeField] GameObject unfinishedPrefab;
    [SerializeField] GameObject finishedPrefab;  

    public Dictionary<ResourceType, int> Resources { get; } = new Dictionary<ResourceType, int>();
    public List<CostInfo> Cost => cost; 

    public override void InitBuilding()
    {
        base.InitBuilding();
        for (int i = 0; i < cost.Count; i++)
        {
            Resources.Add(cost[i].type, cost[i].count);
        }
        finishedPrefab.SetActive(false);
        unfinishedPrefab.SetActive(true);
        OnBuildingInited?.Invoke();
    }

    private string GetResourcePrefId(ResourceType type)
    {
        return buildingId + "_" + type.saveId;
    }

    public override BuildingReport TryUseResources(List<ResourceType> playerResources, int count)
    {
        BuildingReport result = new BuildingReport();
        for(int i = 0; i < playerResources.Count; i++ )
        {
            if (Resources.TryGetValue(playerResources[i], out int needCount))
            {
                ResourceType type = playerResources[i];
                int playerCount = type.Count;
                if (playerCount > 0)
                {
                    int useCount = Mathf.Min(count, playerCount);
                    useCount = Mathf.Min(useCount, needCount);
                    type.Count -= useCount;
                    Resources[playerResources[i]] -= useCount;
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
            result.buildingFinished = UpdateBuilding();
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
        MapController.Instance.Save(this);
    }


    bool UpdateBuilding()
    {
        bool result = false;
        int finishedResources = 0;
        foreach (var pair in Resources)
        {
            if (pair.Value == 0)
            {
                finishedResources++;
            }
        }
        if(finishedResources == Resources.Count)
        {
            result = true;
            FinishBuilding();
        }
        MapController.Instance.UpdateCompletionProgress();
        return result;
    }

    void FinishBuilding()
    {        
        unfinishedPrefab.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCirc).OnComplete(CreateBuilding);        
    }

    void CreateBuilding()
    {
        unfinishedPrefab.gameObject.SetActive(false);
        finishedPrefab.gameObject.SetActive(true);
        Vector3 targetScale = finishedPrefab.transform.localScale;
        finishedPrefab.transform.localScale = Vector3.zero;
        finishedPrefab.transform.DOScale(targetScale, 0.3f).SetEase(Ease.OutCirc);
        finishedPrefab.transform.DOPunchPosition(Vector3.up * 0.8f, 0.5f, 3);
    }

    public override float GetCompletionProgress()
    {
        float result = 0f;
        
        foreach (var pair in Resources)
        {
            for (int i = 0; i < cost.Count; i++)
            {
                if (cost[i].type == pair.Key)
                {
                    result += (1f - (float)pair.Value / (float)cost[i].count);                   
                    break;
                }
            }          
        }
        result /= (float)cost.Count;        
        return result;
    }

    public override JSONNode GetSaveData()
    {
        JSONNode jsonObject = base.GetSaveData();       
        for (int i = 0; i < cost.Count; i++)
        {
            jsonObject.Add(cost[i].type.ToString(), Resources[cost[i].type]);
        }
        return jsonObject;
    }

    public override void Load(JSONNode loadData)
    {
        base.Load(loadData);
        for (int i = 0; i < cost.Count; i++)
        {
            if (loadData.HasKey(cost[i].type.ToString()))
            {
                ResourceType resourceType = cost[i].type;
                int value = loadData[cost[i].type.ToString()].AsInt;
                Resources[resourceType] = value;
            }        
        }
        UpdateBuilding();     
    }
}
