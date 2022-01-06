using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleJSON;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;


public class MapController : SingletonMono<MapController>
{

    public event System.Action<float> OnCompletionProgressUpdate;
    public event System.Action OnMapCompleted; 

    Vector3 BoundsCenter = Vector3.zero;  

    [SerializeField, HideInInspector] List<MapCell> mapCells = new List<MapCell>();
    [SerializeField, HideInInspector] int mapCellId = -1;
    [SerializeField, HideInInspector] int buildingId = -1;
    [SerializeField, HideInInspector] string saveId;

    JSONNode saveDataNode;
    float mapProgress = 0;    

    public List<MapCell> MapCells => mapCells;
    //public List<BaseBuilding> Buildings => buildings;
    string json;
    BaseObject[] objects;

    private void Awake()
    {
        objects = FindObjectsOfType<BaseObject>();
        foreach (var obj in objects)
        {            
            obj.OnRequireSave += BaseObject_OnRequireSave;
        }
        NewLoad();
    }

    private void BaseObject_OnRequireSave()
    {
        SaveBase();
    }

    void OnDisable()
    {
        SaveBase();
    }


    public void SaveBase()
    {
        NewSave();
    }


    private void Update()
    {
        // if()
        // {
        //     NewSave();
        // }
    }


    void NewSave()
    {
        //Debug.Log("Save");

        json = BaseSerialization.SerializeBase(objects);
        //Debug.Log("JSON : " + json);
        PlayerPrefs.SetString("BaseInfo", json);
    }


    void NewLoad()
    {
        //Debug.Log("Load");
        var json = PlayerPrefs.GetString("BaseInfo", null);
        if(!string.IsNullOrEmpty(json))
        {
            //Debug.Log("JSON : " + json);
            BaseSerialization.DeserializeBase(json, objects);
        }
    }


    void BaseObject_OnUpdate()
    {
        //NewSave();
    }


    public void UpdateCompletionProgress()
    {
        float progress = 0;
        //for (int i = 0; i < buildings.Count; i++)
        //{
        //    progress += buildings[i].GetCompletionProgress();           
        //}
        //progress /= (float)buildings.Count;        
        OnCompletionProgressUpdate?.Invoke(progress);
        if (progress >= 1f)
        {
            OnMapCompleted?.Invoke();
        }
    }
}




