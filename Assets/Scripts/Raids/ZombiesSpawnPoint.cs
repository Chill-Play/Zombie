using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesSpawnPoint : MonoBehaviour , IProbability
{
    [SerializeField] Enemy[] zombiePrefabs;
    [SerializeField] Construction spawnPointBarricade;
    [SerializeField] float probability = 1f;

    public Construction SpawnPointBarricade => spawnPointBarricade;

    public float Probability => probability;

    public Enemy GetPrefab()
    {
        return zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
    }


}
