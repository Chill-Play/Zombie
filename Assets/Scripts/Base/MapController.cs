using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleJSON;
using System.IO;
using UnityEngine.SceneManagement;


public class MapController : SingletonMono<MapController>
{

    public event System.Action<float> OnCompletionProgressUpdate;
    public event System.Action OnMapComplited; 

    [SerializeField] LayerMask NavMeshMask;
    [SerializeField] Vector3 BoundsSize = new Vector3(512f, 4000f, 512f); 

    Vector3 BoundsCenter = Vector3.zero;  

    [SerializeField, HideInInspector] List<MapCell> mapCells = new List<MapCell>();
    //[SerializeField, HideInInspector] List<BaseBuilding> buildings = new List<BaseBuilding>();
    [SerializeField, HideInInspector] int mapCellId = -1;
    [SerializeField, HideInInspector] int buildingId = -1;
    [SerializeField, HideInInspector] string saveId;

    JSONNode saveDataNode;
    float mapProgress = 0;    

    public List<MapCell> MapCells => mapCells;
    //public List<BaseBuilding> Buildings => buildings;

    public void ApplyMapChanges()
    {
        //MapCell[] sceneMapCells = FindObjectsOfType<MapCell>();
        //BaseBuilding[] sceneBuildings = FindObjectsOfType<BaseBuilding>(true);

        //mapCells = new List<MapCell>();
        //buildings = new List<BaseBuilding>();
        //for (int i = 0; i < sceneMapCells.Length; i++)
        //{        
        //    if (sceneMapCells[i].SaveId == MapCell.DEFAULT_GRID_ID)
        //    {              
        //        mapCellId++;
        //        sceneMapCells[i].SaveId = "map_cell_" + mapCellId.ToString();                
        //    }
        //    mapCells.Add(sceneMapCells[i]);            
        //}
        //for (int i = 0; i < sceneBuildings.Length; i++)
        //{
        //    buildings.Add(sceneBuildings[i]);
        //    if (sceneBuildings[i].SaveId == BaseBuilding.DEFAULT_BUILDING_ID)
        //    {
                
        //        buildingId++;
        //        sceneBuildings[i].SaveId = "building_" + buildingId.ToString();
        //    }            
        //}

        //saveId = SceneManager.GetActiveScene().name;
    }

    public void ClearPrefs()
    {
        MapCell[] sceneMapCells = FindObjectsOfType<MapCell>();
        //BaseBuilding[] sceneBuildings = FindObjectsOfType<BaseBuilding>(true);
        mapCellId = 0;
        buildingId = 0;
        //for (int i = 0; i < sceneMapCells.Length; i++)
        //{
        //    sceneMapCells[i].SaveId = MapCell.DEFAULT_GRID_ID;
        //}
        //for (int i = 0; i < sceneBuildings.Length; i++)
        //{
        //    sceneBuildings[i].SaveId = BaseBuilding.DEFAULT_BUILDING_ID;
        //}
        string filePath = Path.Combine(Application.persistentDataPath, saveId + ".txt");
        File.Delete(filePath);
    }

    private void Awake()
    {
        //for (int i = 0; i < mapCells.Count; i++)
        //{
        //    mapCells[i].GridIndex = i;
        //    mapCells[i].InitCell();
        //}
        //for (int i = 0; i < buildings.Count; i++)
        //{
        //    buildings[i].InitBuilding();
        //}
        //Load();
        //UpdateNavMesh();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            NewSave();
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            NewLoad();
        }
    }

    string json;
    void NewSave()
    {
        var objects = FindObjectsOfType<BaseObject>();
        json = BaseSerialization.SerializeBase(objects);
        Debug.Log(json);
    }


    void NewLoad()
    {
        var objects = FindObjectsOfType<BaseObject>();
        BaseSerialization.DeserializeBase(json, objects);
    }



    private void Start()
    {
        //UpdateCompletionProgress();
    }

    public void ReplaceMapCell(int gridIndex, MapCell mapCell, bool needToUpdateNavMesh = false)
    {
        mapCells[gridIndex] = mapCell;       
        //mapCells[gridIndex].GridIndex = gridIndex;  
        //mapCells[gridIndex].InitCell();
        if (needToUpdateNavMesh)
        {
            UpdateNavMesh();
        }
    }  

    public void UpdateNavMesh()
    {        
        NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            GetBuildSources(NavMeshMask),
            new Bounds(BoundsCenter, BoundsSize),
            Vector3.zero,
            Quaternion.identity);
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(navMeshData);
        
    }

    List<NavMeshBuildSource> GetBuildSources(LayerMask mask)
    {
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(
            new Bounds(BoundsCenter, BoundsSize),
            mask,
            NavMeshCollectGeometry.PhysicsColliders,
            0,
            new List<NavMeshBuildMarkup>(),
            sources);        
        return sources;
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
            OnMapComplited?.Invoke();
        }
    }
}




