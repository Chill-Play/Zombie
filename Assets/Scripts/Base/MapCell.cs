using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using DG.Tweening;

public class MapCell : MonoBehaviour
{
    [SerializeField] string id;
    int gridId = -1;
    public int GridId { get => gridId; set { gridId = value; } }

    public string Id => id;

    public virtual void InitCell()
    {
        Save();
    }

    public void Save()
    {       
        string key = "map_cell_" + GridId.ToString();
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
