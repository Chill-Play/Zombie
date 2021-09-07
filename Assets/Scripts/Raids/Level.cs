using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : SingletonMono<Level>
{
    public event System.Action<float> OnNoiseLevelChanged;
    public event System.Action OnNoiseLevelExceeded;
    public event System.Action OnHordeDefeated;
    public event System.Action OnLevelEnded;
    public event System.Action OnLevelFailed;

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
    [SerializeField] bool tutorialMode = false;


    List<ZombiesDoorSpawner> doorSpawners;
    List<ResourceSpot> resourceSpots = new List<ResourceSpot>();
    List<Enemy> enemies = new List<Enemy>();

    float noiseLevel;
    float comingTimer;

    bool noiseLevelExceeded;
    bool comingTimerActive;

    int zombieLevel;
    int generation;

    Coroutine spawnWavesCoroutine;
    Squad squad;
    GameplayController gameplayController;

    public float MaxNoiseLevel => maxNoiseLevel;
    public float ComingTimerValue => comingTimer / comingTime;

    private void Awake()
    {
        squad = FindObjectOfType<Squad>();
        
    }


    void Start()
    {
        zombieLevel = LevelController.Instance.CurrentLevel;
    }

    void OnEnable()
    {
        gameplayController = FindObjectOfType<GameplayController>();
        gameplayController.OnReturnedToBase += SpawnPoint_OnReturnedToBase;
        squad.OnPlayerUnitDead += Squad_OnPlayerUnitDead;
    }

    private void Squad_OnPlayerUnitDead()
    {
        gameplayController.OnReturnedToBase -= SpawnPoint_OnReturnedToBase;
        OnLevelFailed?.Invoke();
    }

    void OnDisable()
    {
        squad.OnPlayerUnitDead -= Squad_OnPlayerUnitDead;
        if (gameplayController != null)
        {
            gameplayController.OnReturnedToBase -= SpawnPoint_OnReturnedToBase;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            EndLevel();
        }
        if(comingTimerActive)
        {
            comingTimer -= Time.deltaTime;
            if(comingTimer <= 0)
            {
                var zombies = FindObjectsOfType<Enemy>();
                foreach(var e in zombies)
                {
                   if(!e.IsDead)
                    {
                        e.GoAggressive();
                    }
                }
                SpawnHorde(hordeSize, bigZombiesCount, 0, generation);
                comingTimerActive = false;
            }
        }
    }


    //public void SpawnInDoors()
    //{
    //    var squad = FindObjectOfType<Squad>();
    //    var points = new List<ZombiesDoorSpawner>(doorSpawners);
    //    points.RemoveAll((x) => Vector3.Distance(x.transform.position, squad.transform.position) > 15f);
    //    for (int i = 0; i < hordeSize; i++)
    //    {
    //        Transform spawnPoint = points[Random.Range(0, points.Count)];
    //        Enemy prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
    //        Enemy enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    //        enemy.SetLevel(level);
    //        enemies.Add(enemy);
    //        enemy.GoAggressive();
    //        enemy.GetComponent<IDamagable>().OnDead += Enemy_OnDead;
    //    }
    //}


    public void SpawnHorde(int hordeSize, int bigZombiesCount, int level, int generation)
    {
        hordeSize = (int)((float)hordeSize / 2.5f); //release stuff
        StartCoroutine(SpawnHordeCoroutine(hordeSize, level, generation));

    }


    IEnumerator SpawnHordeCoroutine(int hordeSize, int level, int generation)
    {
        var spawnGroup = 10;
        var spawned = 0;
        List<Transform> spawnPoints = new List<Transform>(zombiesSpawnPoints.Count);

        while(spawned < hordeSize)
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
                if(enemy.TryGetComponent<ZombieLevelStats>(out var stats))
                {
                    stats.SetLevel(level, generation);
                }
                enemies.Add(enemy);
                enemy.GoAggressive();
                enemy.GetComponent<IDamagable>().OnDead += Enemy_OnDead;
            }
            spawned += spawnCount;
            spawnPoints.Clear();
            yield return new WaitForSeconds(0.5f);
        }
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
            generation++;
            SpawnHorde(finalWavesHordeSize, finalWavesBigZombiesCount, 0, generation);
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
        if (spawnWavesCoroutine != null)
        {
            StopCoroutine(spawnWavesCoroutine);
        }
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
        OnLevelEnded?.Invoke();
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
        if (noiseLevelExceeded || tutorialMode) return;
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


    void SpawnPoint_OnReturnedToBase()
    {
        EndLevel();
    }
}
