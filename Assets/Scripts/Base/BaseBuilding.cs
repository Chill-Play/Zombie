using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BaseBuilding : MonoBehaviour, IBuilding, ISaveableMapData
{
    public const string DEFAULT_BUILDING_ID = "none";

    [SerializeField] protected List<GameObject> saveingContent = new List<GameObject>();

    protected List<ISaveableMapData> saveableMapDatas = new List<ISaveableMapData>();

    public event Action<ISaveableMapData> OnSave;

    [SerializeField] protected string buildingId = DEFAULT_BUILDING_ID;
    public virtual string SaveId { get => buildingId; set { buildingId = value; } }


    public virtual void InitBuilding()
    {
        for (int i = 0; i < saveingContent.Count; i++)
        {            
            ISaveableMapData saveableMapData = saveingContent[i].GetComponent<ISaveableMapData>();
            saveableMapDatas.Add(saveableMapData);
            saveableMapData.OnSave += SaveableMapData_OnSave;
        }
    }

    protected void SaveableMapData_OnSave(ISaveableMapData obj)
    {
        MapController.Instance.Save(this);
    }

    public virtual JSONNode GetSaveData()
    {       
        JSONObject jsonObject = new JSONObject();
        for (int i = 0; i < saveableMapDatas.Count; i++)
        {           
            jsonObject.Add("save_data", saveableMapDatas[i].GetSaveData());
        }      
        return jsonObject;
    }

    public virtual void Load(JSONNode loadData)
    {     
        for (int i = 0; i < saveableMapDatas.Count; i++)
        {
            saveableMapDatas[i].Load(loadData);
        }
    }

    public virtual BuildingReport TryUseResources(List<ResourceType> playerResources, int count)
    {
        return new BuildingReport();
    }

    public virtual float GetCompletionProgress()
    {        
        return 1f;
    }
}
