using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ZombieSpawnSettings : IProbability
{
    public Enemy zombiePrefabs;
    public float probability = 1f;

    public float Probability => probability;
}

public class ZombiesSpawnPoint : MonoBehaviour , IProbability
{
    [SerializeField] List<ZombieSpawnSettings> zombieSpawnSettings = new List<ZombieSpawnSettings>();
    [SerializeField] Construction spawnPointBarricade;
    [SerializeField] float probability = 1f;
    [SerializeField] float probabilityPenalty = 0f;

    public Construction SpawnPointBarricade => spawnPointBarricade;

    float currentProbability;

    public float Probability => currentProbability;

    private void Awake()
    {
        currentProbability = probability;
    }

    public Enemy GetPrefab()
    {
        return zombieSpawnSettings[ProbabilityHelper.Choose(zombieSpawnSettings as IEnumerable<IProbability>)].zombiePrefabs;
    }

    public void ApplyPenalty()
    {
        currentProbability -= probabilityPenalty;
    }


}
