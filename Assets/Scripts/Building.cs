using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuilding
{
    [System.Serializable]
    public struct CostInfo
    {
        public ResourceType type;
        public int count;
    }
    [SerializeField] string buildingId;
    [SerializeField] List<CostInfo> cost = new List<CostInfo>();
    [SerializeField] Transform resourcesLayout;
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] GameObject unfinishedPrefab;
    [SerializeField] GameObject finishedPrefab;


    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    Dictionary<ResourceType, ResourceBar> resourceBars = new Dictionary<ResourceType, ResourceBar>();

    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i < cost.Count; i++)
       {
            int costCount = cost[i].count;
            string pref = GetResourcePrefId(cost[i].type);
            if (PlayerPrefs.HasKey(pref))
            {
                costCount = PlayerPrefs.GetInt(pref);
            }
            resources.Add(cost[i].type, costCount);
            ResourceBar bar = Instantiate(resourceBarPrefab, resourcesLayout);
            bar.Setup(cost[i].type, cost[i].count);
            resourceBars.Add(cost[i].type, bar);
       }

       UpdateBuilding();
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
            SaveBuilding();
        }
        result.buildingFinished = UpdateBuilding();
        return result;
    }


    void CreateResourceAnimation(ResourceType type, int i)
    {
        Player player = GameplayController.Instance.playerInstance;
        Resource instance = Instantiate(type.defaultPrefab, player.transform.position, Quaternion.LookRotation(Random.insideUnitSphere));
        instance.GetComponent<Rigidbody>().isKinematic = true;
        instance.transform.DOJump(transform.position, 1f, 1, 0.3f).OnComplete(() => Destroy(instance)).SetDelay((float)i * 0.1f);
    }


    void SaveBuilding()
    {
        foreach (var pair in resources)
        {
            string pref = GetResourcePrefId(pair.Key);
            PlayerPrefs.SetInt(pref, pair.Value); 
        }
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
}
