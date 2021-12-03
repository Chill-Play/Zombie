using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesSpawnPoint : MonoBehaviour
{
    [SerializeField] Enemy[] zombiePrefabs;
    [SerializeField] Construction spawnPointBarricade;


    public Construction SpawnPointBarricade => spawnPointBarricade;


    public Enemy GetPrefab()
    {
        return zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
    }


}
