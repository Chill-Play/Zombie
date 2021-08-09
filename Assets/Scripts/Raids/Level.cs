using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : SingletonMono<Level>
{
    public event System.Action<float> OnNoiseLevelChanged;
    public event System.Action OnNoiseLevelExceeded;
    public event System.Action OnHordeDefeated;

    [SerializeField] List<Transform> zombiesSpawnPoints;
    [SerializeField] Enemy[] zombiePrefabs;
    [SerializeField] Enemy[] bigZombiesPrefabs;
    [SerializeField] float maxNoiseLevel = 100f;
    [SerializeField] float comingTime = 10f;
    [SerializeField] int maxHordeSpawnPoints = 4;
    [SerializeField] int minHordeSpawnPoints = 2;
    [SerializeField] int hordeSize = 30;
    [SerializeField] int bigZombiesCount = 5;

    [SerializeField] float timeBeforeFinalWaves = 1f;
    [SerializeField] float finalWavesRate = 5f;
    [SerializeField] int finalWavesHordeSize = 5;
    [SerializeField] int finalWavesBigZombiesCount = 5;
    


    List<ResourceSpot> resourceSpots = new List<ResourceSpot>();
    List<Enemy> enemies = new List<Enemy>();

    float noiseLevel;
    float comingTimer;

    bool noiseLevelExceeded;
    bool comingTimerActive;

    int finalWaveLevel;

    Coroutine spawnWavesCoroutine;

    public float MaxNoiseLevel => maxNoiseLevel;
    public float ComingTimerValue => comingTimer / comingTime;

    void Start()
    {
        //GameplayController.Instance.playerInstance.GetComponent<UnitHealth>().OnDead += PlayerInstance_OnDead;
    }

    private void PlayerInstance_OnDead()
    {
        UIController.Instance.ShowFailedScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnHorde(hordeSize, bigZombiesCount, 0);
        }
        if(comingTimerActive)
        {
            comingTimer -= Time.deltaTime;
            if(comingTimer <= 0)
            {

                SpawnHorde(hordeSize, bigZombiesCount, 0);
                comingTimerActive = false;
            }
        }
    }


    public void SpawnHorde(int hordeSize, int bigZombiesCount, int level)
    {
        int spawnPointsCount = Random.Range(minHordeSpawnPoints, maxHordeSpawnPoints + 1);

        List<Transform> spawnPoints = new List<Transform>();
        spawnPoints.AddRange(zombiesSpawnPoints);
        spawnPoints.Shuffle();

        for (int i = 0; i < hordeSize; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPointsCount)];
            Enemy prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
            Enemy enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            enemy.SetLevel(level);
            enemies.Add(enemy);
            enemy.GetComponent<IDamagable>().OnDead += Enemy_OnDead;
        }


        //for (int i = 0; i < bigZombiesCount; i++)
        //{
        //    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPointsCount)];
        //    Enemy prefab = bigZombiesPrefabs[Random.Range(0, zombiePrefabs.Length)];
        //    Enemy enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        //    enemy.SetLevel(level);
        //    enemies.Add(enemy);
        //    //enemy.OnDead += Enemy_OnDead;
        //}
    }

    private void Enemy_OnDead(EventMessage<Empty> message)
    {
        var health = message.sender as UnitHealth;
        health.OnDead -= Enemy_OnDead;
        enemies.Remove(health.GetComponent<Enemy>());

        if (enemies.Count == 0 && spawnWavesCoroutine == null)
        {
            OnHordeDefeated?.Invoke();
            FindObjectOfType<SpawnPoint>().IsReturningToBase = true;
            spawnWavesCoroutine = StartCoroutine(SpawningFinalWaves());
        }
    }


    IEnumerator SpawningFinalWaves()
    {
        yield return new WaitForSeconds(timeBeforeFinalWaves);

        while(true)
        {
            yield return new WaitForSeconds(finalWavesRate);
            finalWaveLevel++;
            SpawnHorde(finalWavesHordeSize, finalWavesBigZombiesCount, finalWaveLevel);
        }
    }

    bool levelEnded;
    public void EndLevel()
    {
        if(levelEnded)
        {
            return;
        }
        levelEnded = true;
        StopCoroutine(spawnWavesCoroutine);
        Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
        foreach(PlayerBackpack backpack in FindObjectsOfType<PlayerBackpack>())
        {
            foreach(var pair in backpack.Resources)
            {
                if(resources.ContainsKey(pair.Key))
                {
                    resources[pair.Key] += pair.Value;
                }
                else
                {
                    resources.Add(pair.Key, pair.Value);
                }
            }
        }
        UIController.Instance.ShowFinishScreen(resources);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Stop();
        }
    }


    public void RegisterResourceSpot(ResourceSpot spot)
    {
        resourceSpots.Add(spot);
        spot.OnSpotUsed += Spot_OnSpotUsed;
    }


    public void AddNoiseLevel(float noise)
    {
        if (noiseLevelExceeded) return;
        noiseLevel += noise;
        if(noiseLevel >= maxNoiseLevel)
        {
            noiseLevelExceeded = true;
            noiseLevel = maxNoiseLevel;
            comingTimerActive = true;
            comingTimer = comingTime;

            OnNoiseLevelExceeded?.Invoke();
        }
        OnNoiseLevelChanged?.Invoke(noiseLevel);
    }


    private void Spot_OnSpotUsed(ResourceSpot obj)
    {
        obj.OnSpotUsed -= Spot_OnSpotUsed;
        resourceSpots.Remove(obj);
    }
}
