using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleJSON;
using System.IO;
using UnityEngine.SceneManagement;


public class MapController : SingletonMono<MapController>
{
    [System.Serializable]
    public struct GridPoint
    {
        public int x;
        public int y;

        public GridPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public event System.Action<float> OnCompletionProgressUpdate;
    public event System.Action OnMapComplited;

  
    [SerializeField] Vector2 mapExtends;
    [SerializeField] Vector2 cellSize;

    [SerializeField] LayerMask NavMeshMask;

    Vector3 BoundsCenter = Vector3.zero;
    Vector3 BoundsSize = new Vector3(512f, 4000f, 512f);


    [SerializeField, HideInInspector] List<MapCell> mapCells = new List<MapCell>();
    [SerializeField, HideInInspector] List<Building> buildings = new List<Building>();
    [SerializeField, HideInInspector] int mapCellId = -1;
    
    JSONNode saveDataNode;

    float mapProgress = 0;
    string saveId;

    public List<MapCell> MapCells => mapCells;

    public void ApplyMapChanges()
    {
        MapCell[] sceneMapCells = FindObjectsOfType<MapCell>();
        Building[] sceneBuildings = FindObjectsOfType<Building>(true);

        mapCells = new List<MapCell>();
        buildings = new List<Building>();
        for (int i = 0; i < sceneMapCells.Length; i++)
        {
            if (sceneMapCells[i].GridId == MapCell.DEFAULT_GRID_ID)
            {              
                mapCellId++;
                sceneMapCells[i].GridId = "map_cell_" + mapCellId.ToString();                
            }
            mapCells.Add(sceneMapCells[i]);            
        }
        for (int i = 0; i < sceneBuildings.Length; i++)
        {
            buildings.Add(sceneBuildings[i]);
        }

        saveId = SceneManager.GetActiveScene().name;
    }

    public void ClearPrefs()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveId + ".json");
        File.Delete(filePath);
    }

    private void Awake()
    {
        for (int i = 0; i < mapCells.Count; i++)
        {
            if (mapCells[i] != null)
            {
                mapCells[i].GridIndex = i;
                mapCells[i].InitCell();              
            }
        }
        Load();
        UpdateNavMesh();
    }

    void Load()
    {
        saveDataNode = new JSONObject();
        string filePath = Path.Combine(Application.persistentDataPath, saveId + ".txt");
        bool isSaveExist = File.Exists(filePath);        
        if (isSaveExist)
        {
            string loadData = File.ReadAllText(filePath);
            JSONNode mainNode = JSON.Parse(loadData);
            for (int i = 0; i < mapCells.Count; i++)
            {
                MapCell mapCell = mapCells[i];
                if (mapCell != null)
                {
                    if (mainNode.HasKey(mapCell.GridId))
                    {
                        saveDataNode.Add(mapCell.GridId, mainNode[mapCell.GridId]);                        
                    }
                    else
                    {
                        saveDataNode.Add(mapCell.GridId, "");
                    }
                    mapCell.Load(saveDataNode[mapCell.GridId]);
                }
            }        
        }
        else
        {
            for (int i = 0; i < mapCells.Count; i++)
            {
                MapCell mapCell = mapCells[i];
                if (mapCell != null)
                {
                    saveDataNode.Add(mapCell.GridId, "");
                }
                mapCell.Load(saveDataNode[mapCell.GridId]);
            }
        }

    }


    public void Save(MapCell mapCell)
    {
        saveDataNode[mapCell.GridId] = mapCell.GetSaveData();
        string filePath = Path.Combine(Application.persistentDataPath, saveId + ".txt");
        File.WriteAllText(filePath, saveDataNode.ToString());
    }

    private void Start()
    {
        UpdateCompletionProgress();
    }

    public void ReplaceMapCell(int gridIndex, MapCell mapCell, bool needToUpdateNavMesh = false)
    {
        mapCells[gridIndex] = mapCell;       
        mapCells[gridIndex].GridIndex = gridIndex;  
        mapCells[gridIndex].InitCell();
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
        for (int i = 0; i < buildings.Count; i++)
        {
            progress += buildings[i].GetCompletionProgress();           
        }
        progress /= (float)buildings.Count;        
        OnCompletionProgressUpdate?.Invoke(progress);
        if (progress >= 1f)
        {
            OnMapComplited?.Invoke();
        }
    }
}




