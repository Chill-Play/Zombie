using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool TryUseResources(List<ResourceType> playerResources, int count)
    {
        bool result = false;
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
                    result = true;
                    if (useCount > 0)
                    {
                        CreateResourceAnimation(playerResources[i], i);
                    }
                }
            }
        }
        if (result)
        {
            ResourcesController.Instance.UpdateResources();
            SaveBuilding();
        }
        UpdateBuilding();
        return result;
    }


    void CreateResourceAnimation(ResourceType type, int i)
    {
        return;
        //GameObject player = GameplayController.Instance.playerInstance;
        //Resource instance = Instantiate(type.defaultPrefab, player.transform.position, Quaternion.LookRotation(Random.insideUnitSphere));
        //instance.GetComponent<Rigidbody>().isKinematic = true;
        //instance.transform.DOJump(transform.position, 1f, 1, 0.3f).OnComplete(() => Destroy(instance)).SetDelay((float)i * 0.1f);
    }


    void SaveBuilding()
    {
        foreach (var pair in resources)
        {
            string pref = GetResourcePrefId(pair.Key);
            PlayerPrefs.SetInt(pref, pair.Value);
        }
    }


    void UpdateBuilding()
    {
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
            FinishBuilding();
        }
    }

    void FinishBuilding()
    {
        unfinishedPrefab.gameObject.SetActive(false);
        finishedPrefab.gameObject.SetActive(true);
    }
}
