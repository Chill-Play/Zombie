using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SimpleJSON;
using System.IO;


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

    [SerializeField, HideInInspector] Vector2 bLPoint;
    [SerializeField, HideInInspector] Vector2 tRPoint;

    [SerializeField, HideInInspector] int sizeX;
    [SerializeField, HideInInspector] int sizeY;

    public void ApplyMapChanges()
    {
        MapCell[] sceneMapCells = FindObjectsOfType<MapCell>();

        bLPoint = new Vector2(transform.position.x - mapExtends.x, transform.position.z - mapExtends.y);
        tRPoint = new Vector2(transform.position.x + mapExtends.x, transform.position.z + mapExtends.y);
      
        sizeX = Mathf.CeilToInt((tRPoint.x - bLPoint.x) / cellSize.x);
        sizeY = Mathf.CeilToInt((tRPoint.y - bLPoint.y) / cellSize.y);    
        
        mapCells = new List<MapCell>();
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            mapCells.Add(null);
        }

        for (int i = 0; i < sceneMapCells.Length; i++)
        {
            GridPoint gridPoint = GetGridPoint(sceneMapCells[i].transform.position);
            mapCells[GetIndexByGridPoint(gridPoint)] = sceneMapCells[i];
            sceneMapCells[i].transform.position = GetCellPosition(gridPoint);
        }        
    }

    private void Awake()
    {
        Load();
        for (int i = 0; i < mapCells.Count; i++)
        {
            if (mapCells[i] != null)
            {
                mapCells[i].GridId = i;
                mapCells[i].InitCell();
            }
        }
        UpdateNavMesh();
    }


    public void ReplaceMapCell(int gridId, MapCell mapCell)
    {       
       
        mapCells[gridId] = mapCell;
        mapCell.GridId = gridId;
        mapCell.Save();
        UpdateNavMesh(); 
    } 

    public GridPoint GetGridPoint(Vector3 position)
    {      
        int x = Mathf.FloorToInt((position.x - bLPoint.x) / cellSize.x);
        int y = Mathf.FloorToInt((position.z - bLPoint.y) / cellSize.y);       
        return new GridPoint(x, y);
    }

    public Vector3 GetCellPosition(GridPoint gridPoint)
    {        
        return new Vector3((bLPoint.x + gridPoint.x * cellSize.x + cellSize.x/2f), 0f, (bLPoint.y + gridPoint.y * cellSize.y + cellSize.y / 2f));
    }

    public Vector3 GetCellPosition(int index)
    {
        return GetCellPosition(GetGridPointByIndex(index));
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

    public int GetIndexByGridPoint(GridPoint gridPoint)
    {
        return gridPoint.y * sizeX + gridPoint.x;
    }

    public GridPoint GetGridPointByIndex(int index)
    {
        return new GridPoint(index % sizeX, Mathf.FloorToInt(index / sizeX));
    }

    public void Load()
    {
        for (int i = 0; i < mapCells.Count; i++)
        {
            string gridCellId = "map_cell_" + i;
            if (PlayerPrefs.HasKey(gridCellId))
            {
                string loadInfo = PlayerPrefs.GetString(gridCellId);
                mapCells[i].GridId = i;
                mapCells[i].Load(loadInfo);
            }
        }
    }
}




