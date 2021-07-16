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
       

    [SerializeField] Vector2 mapExtends;
    [SerializeField] Vector2 cellSize;

    [SerializeField] LayerMask NullMask;

    Vector3 BoundsCenter = Vector3.zero;
    Vector3 BoundsSize = new Vector3(512f, 4000f, 512f);

    [SerializeField, HideInInspector] List<MapCell> mapCells = new List<MapCell>();

    [SerializeField, HideInInspector] string sceneName;
    [SerializeField, HideInInspector] string scenePath;

    public void ApplyMapChanges()
    {
        MapCell[] sceneMapCells = FindObjectsOfType<MapCell>();

        mapCells = new List<MapCell>();
        for (int i = 0; i < sceneMapCells.Length; i++)
        {
            mapCells.Add(sceneMapCells[i]);
        }

        Scene scene = SceneManager.GetActiveScene();

        sceneName = scene.name;
        scenePath = scene.path;        
    }

    private void Awake()
    {      
        PlayerPrefs.SetString("active_scene", scenePath);
        Load();
        for (int i = 0; i < mapCells.Count; i++)
        {
            if (mapCells[i] != null)
            {
                mapCells[i].GridId = sceneName + "_" + i;
                mapCells[i].GridIndex = i;
                mapCells[i].InitCell();
            }
        }
        UpdateNavMesh();
    }


    public void ReplaceMapCell(int gridId, MapCell mapCell, bool needToUpdateNavMesh = false)
    {   
        mapCells[gridId] = mapCell;
        mapCell.GridId = sceneName + "_" + gridId;
        mapCells[gridId].GridIndex = gridId;
        mapCell.Save();
        if (needToUpdateNavMesh)
        {
            UpdateNavMesh();
        }
    }  

    public void UpdateNavMesh()
    {        
        NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByID(0),
            GetBuildSources(NullMask),
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

    public void Load()
    {
        for (int i = 0; i < mapCells.Count; i++)
        {
            string gridCellId = "map_cell_" + sceneName + "_" + i;
            if (PlayerPrefs.HasKey(gridCellId))
            {
                string loadInfo = PlayerPrefs.GetString(gridCellId);
                mapCells[i].GridId = sceneName + "_" + i;
                mapCells[i].GridIndex = i;
                mapCells[i].Load(loadInfo);
            }
        }
    }
}




