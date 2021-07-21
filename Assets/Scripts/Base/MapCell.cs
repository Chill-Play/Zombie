using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using DG.Tweening;

public class MapCell : MonoBehaviour
{   
    string gridId;
   

    public string GridId { get => gridId; set { gridId = value; } }
    public int GridIndex { get; set; }

   


    public virtual void InitCell()
    {
        Save();
    }

    public void Save()
    {       
        string key = "map_cell_" + GridId;
        string saveInfo = GetSaveData().ToString();       
        PlayerPrefs.SetString(key, saveInfo);       
    }

    public virtual JSONObject GetSaveData()
    {
        JSONObject jsonObject = new JSONObject();       
        return jsonObject;
    }

    public virtual void Load(string loadData)
    {

    }

    public virtual void Build(System.Action<MapCell> OnBuildingComplete)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc).OnComplete(() => OnBuildingComplete(this));       
    }
}
