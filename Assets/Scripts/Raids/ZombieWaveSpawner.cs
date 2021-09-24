using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde
{
    public event System.Action OnHordeDefeated;

    List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        enemy.GetComponent<IDamagable>().OnDead += Enemy_OnDead;
    }

    private void Enemy_OnDead(EventMessage<Empty> message)
    {
        var health = message.sender as UnitHealth;
        health.OnDead -= Enemy_OnDead;
        enemies.Remove(health.GetComponent<Enemy>());

        if (enemies.Count == 0)
        {
            OnHordeDefeated?.Invoke();    
        }
    }
}

public class ZombieWaveSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> zombiesSpawnPoints;
    [SerializeField] Enemy[] zombiePrefabs;
    [SerializeField] Enemy[] bigZombiesPrefabs;
   
    Coroutine spawnWavesCoroutine;

    public Horde SpawnHorde(int hordeSize, int level, int generation)
    {
        Horde horde = new Horde();
        hordeSize = (int)((float)hordeSize / 2.5f); //release stuff
        StartCoroutine(SpawnHordeCoroutine(horde, hordeSize, level, generation));
        return horde;
    }

    IEnumerator SpawnHordeCoroutine(Horde horde , int hordeSize, int level, int generation)
    {
        var spawnGroup = 10;
        var spawned = 0;
        List<Transform> spawnPoints = new List<Transform>(zombiesSpawnPoints.Count);

        while (spawned < hordeSize)
        {
            var spawnCount = Mathf.Min(spawnGroup, hordeSize - spawned);
            var squad = FindObjectOfType<Squad>();
            spawnPoints.AddRange(zombiesSpawnPoints);
            spawnPoints.Shuffle();
            var points = spawnPoints;
            points.RemoveAll((x) => Mathf.Abs(squad.transform.position.z - x.position.z) < 30f && Mathf.Abs(squad.transform.position.x - x.position.x) < 10f);
            for (int i = 0; i < spawnCount; i++)
            {
                Transform spawnPoint = points[Random.Range(0, points.Count)];
                Enemy prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
                Enemy enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                if (enemy.TryGetComponent<ZombieLevelStats>(out var stats))
                {
                    stats.SetLevel(level, generation);
                }
                horde.AddEnemy(enemy);
                enemy.GoAggressive();              
            }
            spawned += spawnCount;
            spawnPoints.Clear();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
