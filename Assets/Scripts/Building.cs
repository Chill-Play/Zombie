using DG.Tweening;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding, ISavableMapData
{
    public const string DEFAULT_BUILDING_ID = "none";  

    [System.Serializable]
    public struct CostInfo
    {
        public ResourceType type;
        public int count;
    }
    [SerializeField, HideInInspector] protected string buildingId = DEFAULT_BUILDING_ID;  
    [SerializeField] List<CostInfo> cost = new List<CostInfo>();
    [SerializeField] Transform resourcesLayout;
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] GameObject unfinishedPrefab;
    [SerializeField] GameObject finishedPrefab;  

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    Dictionary<ResourceType, ResourceBar> resourceBars = new Dictionary<ResourceType, ResourceBar>();

    public string SaveId { get => buildingId; set { buildingId = value; } }

    
    public void InitBuilding()
    {
      
        for (int i = 0; i < cost.Count; i++)
       {           
            resources.Add(cost[i].type, cost[i].count);
            ResourceBar bar = Instantiate(resourceBarPrefab, resourcesLayout);
            bar.Setup(cost[i].type, cost[i].count);
            resourceBars.Add(cost[i].type, bar);
       }     
    }

    private string GetResourcePrefId(ResourceType type)
    {
        return buildingId + "_" + type.saveId;
    }

    public BuildingReport TryUseResources(List<ResourceType> playerResources, int count)
    {
        BuildingReport result = new BuildingReport();
        for(int i = 0; i < playerResources.Count; i++ )
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
        foreach (var pair in resources)
        {
            resourceBars[pair.Key].UpdateValue(pair.Value);
            if (pair.Value == 0)
            {
                resourceBars[pair.Key].gameObject.SetActive(false);
                finishedResources++;
            }
        }
        if(finishedResources == resources.Count)
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
        finishedPrefab.transform.localScale = Vector3.zero;
        finishedPrefab.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc);
        finishedPrefab.transform.DOPunchPosition(Vector3.up * 0.8f, 0.5f, 3);
    }

    public float GetCompletionProgress()
    {
        float result = 0f;
        
        foreach (var pair in resources)
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

    public JSONNode GetSaveData()
    {
        JSONNode jsonObject = new JSONObject();       
        for (int i = 0; i < cost.Count; i++)
        {
            jsonObject.Add(cost[i].type.ToString(), resources[cost[i].type]);
        }
        return jsonObject;
    }

    public void Load(JSONNode loadData)
    {
       
        for (int i = 0; i < cost.Count; i++)
        {
            if (loadData.HasKey(cost[i].type.ToString()))
            {
                resources[cost[i].type] = loadData[cost[i].type.ToString()].AsInt;
            }
        }
        UpdateBuilding();
    }
}
