using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BaseBuilding : MonoBehaviour, IBuilding, ISaveableMapData
{
    public virtual string SaveId { get; set; }

    public virtual JSONNode GetSaveData()
    {
        return new JSONObject();
    }

    public virtual void Load(JSONNode loadData)
    {
       
    }

    public virtual BuildingReport TryUseResources(List<ResourceType> playerResources, int count)
    {
        return new BuildingReport();
    }
}
