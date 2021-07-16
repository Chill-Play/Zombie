using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SimpleJSON;


public class SellingMapCell : MapCell, IBuilding
{
    [System.Serializable]
    public struct CostInfo
    {
        public ResourceType type;
        public int count;
    }

    [SerializeField] Transform resourcesLayout;
    [SerializeField] ResourceBar resourceBarPrefab;

    [SerializeField] List<CostInfo> cost = new List<CostInfo>();
    [SerializeField] Transform sellingCellContent;
    [SerializeField] MapCell buyingContent;

    Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    Dictionary<ResourceType, ResourceBar> resourceBars = new Dictionary<ResourceType, ResourceBar>();

    public override void InitCell()
    {        
        for (int i = 0; i < cost.Count; i++)
        {
            int costCount = cost[i].count;

            ResourceBar bar = Instantiate(resourceBarPrefab, resourcesLayout);
            if (!resources.ContainsKey(cost[i].type))
            {
                resources.Add(cost[i].type, costCount);
            }
            bar.Setup(cost[i].type, resources[cost[i].type]);
            resourceBars.Add(cost[i].type, bar);
        }        
        base.InitCell();
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
            Save();
        }
        result.buildingFinished = UpdateBuilding();
        return result;
    }

    void CreateResourceAnimation(ResourceType type, int i)
    {
        Player player = GameplayController.Instance.playerInstance;
        Resource instance = Instantiate(type.defaultPrefab, player.transform.position, Quaternion.LookRotation(UnityEngine.Random.insideUnitSphere));
        instance.GetComponent<Rigidbody>().isKinematic = true;
        instance.transform.DOJump(transform.position, 1f, 1, 0.3f).OnComplete(() => Destroy(instance)).SetDelay((float)i * 0.1f);
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
        if (finishedResources == resources.Count)
        {
            result = true;
            Save();
            FinishBuilding();
        }
        return result;
    }

    void FinishBuilding()
    {
        sellingCellContent.DOScale(Vector3.zero,0.3f).SetEase(Ease.InCirc).OnComplete(CreateNewCell);
        UIBuyingPopUpText.Instance.SpawnText(transform.position + 0.5f * Vector3.up);
    }

    void CreateNewCell()
    {
        sellingCellContent.gameObject.SetActive(false);
        buyingContent.gameObject.SetActive(true);
        buyingContent.Build((x) => MapController.Instance.ReplaceMapCell(GridIndex, x, true));         
    }

   

    public override JSONObject GetSaveData()
    {
        JSONObject jsonObject = base.GetSaveData();        
        jsonObject.Add("selling_cell", "yes");
        for (int i = 0; i < cost.Count; i++)
        {
            jsonObject.Add(cost[i].type.ToString(), resources[cost[i].type]);
        }
        return jsonObject;
    }

    public override void Load(string loadData)
    {
        base.Load(loadData);
        JSONNode jsonNode = JSON.Parse(loadData);
        if (jsonNode.HasKey("selling_cell"))
        {
            sellingCellContent.gameObject.SetActive(true);
            for (int i = 0; i < cost.Count; i++)
            {
                resources[cost[i].type] = jsonNode[cost[i].type.ToString()];
            }
        }
        else
        {
            sellingCellContent.gameObject.SetActive(false);
            buyingContent.gameObject.SetActive(true);
            buyingContent.Load(loadData);
            MapController.Instance.ReplaceMapCell(GridIndex, buyingContent);
        }
    }  
}
