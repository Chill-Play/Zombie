using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZombieSpawnPoint : MonoBehaviour
{
    public event System.Action<TutorialZombieSpawnPoint> OnWaveDefeated;

    [System.Serializable]
    class ZombieSpawnParam
    {
        public Enemy enemy;
        public int count;

        [HideInInspector] public int spawnedCount;
    }

    [SerializeField] List<ZombieSpawnParam> zombieSpawnParams = new List<ZombieSpawnParam>();
    [SerializeField] int spawnRate = 5;
    [SerializeField] ConditionTrigger conditionTrigger;

    bool spawned = false;

    List<Enemy> enemies = new List<Enemy>();
    List<ZombieSpawnParam> zombieList = new List<ZombieSpawnParam>();


    private void Awake()
    {
        if (conditionTrigger != null)
        {
            conditionTrigger.OnTrigger += SpawnHorde;
        }
    }

    void SpawnHorde()
    {
        for (int i = 0; i < zombieSpawnParams.Count; i++)
        {
            zombieList.Add(zombieSpawnParams[i]);
        }
        StartCoroutine(SpawnHordeCoroutine());
    }

    IEnumerator SpawnHordeCoroutine()
    {
        while (!spawned)
        {
            for (int i = 0; i < spawnRate; i++)
            {
                int idx = Random.Range(0, zombieList.Count);
                Enemy enemy = Instantiate(zombieList[idx].enemy, transform.position, Quaternion.identity);
                enemies.Add(enemy);
                enemy.GoAggressive();
                enemy.GetComponent<IDamagable>().OnDead += Enemy_OnDead; 
                zombieList[idx].spawnedCount++;
                if (zombieList[idx].spawnedCount == zombieList[idx].count)
                {
                    zombieList.RemoveAt(idx);
                    if (zombieList.Count == 0)
                    {
                        spawned = true;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Enemy_OnDead(EventMessage<Empty> obj)
    {
        var health = obj.sender as UnitHealth;
        health.OnDead -= Enemy_OnDead;
        enemies.Remove(health.GetComponent<Enemy>());
        if (enemies.Count == 0 && spawned)
        {
            OnWaveDefeated?.Invoke(this);
        }
    }
}
