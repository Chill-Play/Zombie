using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde
{   
    public event System.Action OnHordeDefeated;

    List<Enemy> enemies = new List<Enemy>();

    public List<Enemy> Enemies => enemies;

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
    [SerializeField] Enemy[] zombiePrefabs;  

    ZombiesSpawnPoint[] zombiesSpawnPoints;
    List<Horde> hordes = new List<Horde>();

    Coroutine spawnHordeCoroutine;
    HordeController hordeController;
    

    private void Awake()
    {
        zombiesSpawnPoints = FindObjectsOfType<ZombiesSpawnPoint>();
        hordeController = FindObjectOfType<HordeController>();
    }

    public Horde SpawnHorde(int hordeSize, int level, int generation)
    {
        Horde horde = new Horde();
        hordeSize = (int)((float)hordeSize); 
        spawnHordeCoroutine = StartCoroutine(SpawnHordeCoroutine(horde, hordeSize, level, generation));
        hordes.Add(horde);
        horde.OnHordeDefeated += (() => Horde_OnHordeDefeated(horde));
        return horde;
    }

    private void Horde_OnHordeDefeated(Horde horde)
    {
        hordes.Remove(horde);
    }

    IEnumerator SpawnHordeCoroutine(Horde horde, int hordeSize, int level, int generation)
    {
        var spawnGroup = 10;
        var spawned = 0;
        List<ZombiesSpawnPoint> spawnPoints = new List<ZombiesSpawnPoint>(zombiesSpawnPoints.Length);

        while (spawned < hordeSize)
        {
            var spawnCount = Mathf.Min(spawnGroup, hordeSize - spawned);
            var squad = FindObjectOfType<Squad>();
            spawnPoints.AddRange(zombiesSpawnPoints);
            spawnPoints.Shuffle();
            var points = spawnPoints;
            points.RemoveAll((x) => (Mathf.Abs(squad.transform.position.z - x.transform.position.z) < 30f 
            && Mathf.Abs(squad.transform.position.x - x.transform.position.x) < 10f) || (x.SpawnPointBarricade != null && x.SpawnPointBarricade.Constructed));
            if (points.Count > 0) // bad fix, can break game
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    ZombiesSpawnPoint spawnPoint = points[Random.Range(0, points.Count)];
                    Enemy prefab = spawnPoint.GetPrefab();
                    Enemy enemy = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
                    if (enemy.TryGetComponent<ZombieLevelStats>(out var stats))
                    {
                        stats.SetLevel(level, generation);
                    }                  
                    horde.AddEnemy(enemy);
                    hordeController.AddAgent(enemy); ///////////////////////
                    enemy.GoAggressive();
                    AgroActivator agroActivator = enemy.GetComponent<AgroActivator>();
                    if (agroActivator != null)
                    {
                        if (spawnPoint.SpawnPointBarricade != null)
                        {
                            
                        }
                    }
                }

            }
            spawned += spawnCount;
            spawnPoints.Clear();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StopSpawning()
    {
        if (spawnHordeCoroutine != null)
        {
            StopAllCoroutines();
        }
    }

    public void ExecudeHorde(Horde horde)
    {
        for (int i = 0; i < horde.Enemies.Count; i++)
        {      
            horde.Enemies[i].GetComponent<UnitHealth>().Damage(new DamageInfo(Vector3.forward, 999999f));
        }
    }

    public void ExecudeAll()
    {
        for (int i = 0; i < hordes.Count; i++)
        {
            ExecudeHorde(hordes[i]);
        }    
    }
}
