using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using DG.Tweening;
using System;

public class MapCell : MonoBehaviour, ISaveableMapData
{
    public const string DEFAULT_GRID_ID = "none";

    [SerializeField, HideInInspector] protected string gridId = DEFAULT_GRID_ID;

    public event Action<ISaveableMapData> OnSave;

    public string SaveId { get => gridId; set { gridId = value; } }
    public int GridIndex { get; set; }
   

    public virtual void InitCell()
    {
        
    }

    public virtual JSONNode GetSaveData()
    {
        JSONNode jsonObject = new JSONObject();       
        return jsonObject;
    }

    public virtual void Load(JSONNode loadData)
    {

    }

    public virtual void Build(System.Action<MapCell> OnBuildingComplete)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => OnBuildingComplete(this));       
    }
}
