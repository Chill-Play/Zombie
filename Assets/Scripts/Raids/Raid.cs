using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raid : MonoBehaviour
{
    public event System.Action OnHordeDefeated;

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


    public float ComingTimerValue => timer != null ? timer.RemainingTime / timer.TimerTime : 0f;


    private void Awake()
    {
        Level.Instance.OnLevelStarted += Instance_OnLevelStarted;
        Level.Instance.OnLevelEnded += Instance_OnLevelEnded;
        Level.Instance.OnLevelFailed += Instance_OnLevelEnded;
        reviveController = FindObjectOfType<ReviveController>();
        reviveController.OnRevive += ReviveController_OnRevive;
    }

    private void Instance_OnLevelStarted()
    {       
        zombieLevel = ZombiesLevelController.Instance.LevelsPlayed;
        ZombiesLevelController.Instance.RaidStarted();        

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
        mainHorde = zombieWaveSpawner.SpawnHorde(hordeSize, 0, generation);
        mainHorde.OnHordeDefeated += MainHorde_OnHordeDefeated;    
    }

    private void MainHorde_OnHordeDefeated()
    {        
        FindObjectOfType<SpawnPoint>().IsReturningToBase = true;
        OnHordeDefeated?.Invoke();
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