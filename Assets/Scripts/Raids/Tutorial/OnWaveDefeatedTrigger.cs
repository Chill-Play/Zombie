using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWaveDefeatedTrigger : ConditionTrigger
{
    [SerializeField] List<TutorialZombieSpawnPoint> spawnPoints = new List<TutorialZombieSpawnPoint>();

    int spawnPointsCount = 0;

    private void Awake()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnPoints[i].OnWaveDefeated += OnWaveDefeatedTrigger_OnWaveDefeated; ;
        }
        spawnPointsCount = spawnPoints.Count;
    }

    private void OnWaveDefeatedTrigger_OnWaveDefeated(TutorialZombieSpawnPoint obj)
    {
        spawnPointsCount--;
        if (spawnPointsCount == 0)
        {
            InvokeEvent();
            spawnPoints.Clear();
        }        
    } 
}
