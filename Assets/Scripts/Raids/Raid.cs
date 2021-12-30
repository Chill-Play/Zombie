using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raid : MonoBehaviour
{
    public event System.Action OnHordeDefeated;
    public event System.Action OnHordeBegin;

    [SerializeField] float comingTime = 10f;
    [SerializeField] int hordeSize = 30;
    [SerializeField] float timeBeforeFinalWaves = 1f;
    [SerializeField] float finalWavesRate = 5f;
    [SerializeField] int finalWavesHordeSize = 5;

    int generation = 0; 
    int zombieLevel = 0;
    NoiseController noiseController;
    ZombieWaveSpawner zombieWaveSpawner;
    ReviveController reviveController;
    HC_Timer timer;
    Horde mainHorde;
    Coroutine spawnWavesCoroutine;
    Helicopter helicopter;
    StarsChest starsChest;
    bool campaign = false; // temp


    public float ComingTimerValue => timer != null ? timer.RemainingTime / timer.TimerTime : 0f;


    private void Awake()
    {
        Level.Instance.OnLevelStarted += Instance_OnLevelStarted;
        Level.Instance.OnLevelEnded += Instance_OnLevelEnded;
        Level.Instance.OnLevelFailed += Instance_OnLevelEnded;
        reviveController = FindObjectOfType<ReviveController>();
        reviveController.OnRevive += ReviveController_OnRevive;
        starsChest = FindObjectOfType<StarsChest>(true);
        campaign = FindObjectOfType<Campaign>() != null;
    }

    private void Start()
    {
        helicopter = FindObjectOfType<Helicopter>();
        if (helicopter != null)
        {           
            helicopter.FlyAway();
        }        
    }

    private void Instance_OnLevelStarted()
    {
        if (campaign) 
        {
            ZombiesLevelController.Instance.CampaignStarted();
        }
        else
        {
            ZombiesLevelController.Instance.RaidStarted();
        }
        noiseController = FindObjectOfType<NoiseController>();
        noiseController.OnNoiseLevelExceeded += NoiseController_OnNoiseLevelExceeded;
        
        zombieWaveSpawner = FindObjectOfType<ZombieWaveSpawner>();
    }
    private void Instance_OnLevelEnded()
    {
        if (spawnWavesCoroutine != null)
        {
            StopCoroutine(spawnWavesCoroutine);
        }
    }

    private void NoiseController_OnNoiseLevelExceeded()
    {
        timer = TimerController.RunTimer(comingTime, OnComingTimerEnd);
    }

    private void OnComingTimerEnd()
    {
        var zombies = FindObjectsOfType<Enemy>();
        foreach (var e in zombies)
        {
            if (!e.IsDead)
            {
                e.GoAggressive();
            }
        }
        OnHordeBegin?.Invoke();
        mainHorde = zombieWaveSpawner.SpawnHorde(hordeSize, 0, generation);
        mainHorde.OnHordeDefeated += MainHorde_OnHordeDefeated;  
        
    }

    private void MainHorde_OnHordeDefeated()
    {
        OnHordeDefeated?.Invoke();  
        if (helicopter != null)
        {
            starsChest.gameObject.SetActive(true);
            helicopter.OnArrived += Helicopter_OnArrived;
            helicopter.FlyBack();
        }
        else
        {
            SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
            spawnPoint.IsReturningToBase = true;
            spawnWavesCoroutine = StartCoroutine(SpawningFinalWaves());
        }
    }



    private void Helicopter_OnArrived()
    {
        Squad squad = FindObjectOfType<Squad>();        
        starsChest.PickupAll(squad.Units[0].transform);
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        spawnPoint.IsReturningToBase = true;
        spawnWavesCoroutine = StartCoroutine(SpawningFinalWaves());
    }

    IEnumerator SpawningFinalWaves()
    {
        yield return new WaitForSeconds(timeBeforeFinalWaves);

        while (true)
        {
            yield return new WaitForSeconds(finalWavesRate);
            generation++;
            zombieWaveSpawner.SpawnHorde(finalWavesHordeSize, 0, generation);
        }
    }

    private void ReviveController_OnRevive()
    {       
        mainHorde.OnHordeDefeated -= MainHorde_OnHordeDefeated;
        if (spawnWavesCoroutine != null)
        {
            StopCoroutine(spawnWavesCoroutine);
        }
        mainHorde = zombieWaveSpawner.SpawnHorde(hordeSize, 0, generation);
        mainHorde.OnHordeDefeated += MainHorde_OnHordeDefeated;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            zombieWaveSpawner.StopSpawning();
            zombieWaveSpawner.ExecudeAll();
        }
    }
}