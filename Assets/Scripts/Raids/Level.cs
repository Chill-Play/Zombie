using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level : SingletonMono<Level>
{
    public event System.Action OnLevelEnded;
    public event System.Action OnLevelFailed;
    public event System.Action OnLevelStarted;


    SurvivorPickup[] survivorPickups;

    GameplayController gameplayController;
    ReviveController reviveController;
    bool levelEnded;

    public int Tries { get; set; } = 0;


    private void Awake()
    {
        survivorPickups = FindObjectsOfType<SurvivorPickup>();        
        reviveController = ReviveController.Instance;
        reviveController.OnRevive += ReviveController_OnRevive;
    }


    void Start()
    {
        OnLevelStarted?.Invoke();   
    }

    void OnEnable()
    {
        gameplayController = GameplayController.Instance;
        gameplayController.OnReturnedToBase += SpawnPoint_OnReturnedToBase;
        gameplayController.OnPlayerUnitDead += GameplayController_OnPlayerUnitDead;
    }

    private void GameplayController_OnPlayerUnitDead()
    {
        gameplayController.OnReturnedToBase -= SpawnPoint_OnReturnedToBase;
        OnLevelFailed?.Invoke();
    }

    void OnDisable()
    {
        if (gameplayController != null)
        {
            gameplayController.OnReturnedToBase -= SpawnPoint_OnReturnedToBase;
            gameplayController.OnPlayerUnitDead -= GameplayController_OnPlayerUnitDead;
        }
    } 
   
    public void EndLevel()
    {       
        if(levelEnded)
        {       
            return;
        }       
        levelEnded = true;
        OnLevelEnded?.Invoke();
    }

    void SpawnPoint_OnReturnedToBase()
    {        
        EndLevel();
    }

    private void ReviveController_OnRevive()
    {
        levelEnded = false;
        gameplayController.OnReturnedToBase += SpawnPoint_OnReturnedToBase;
        gameplayController.OnPlayerUnitDead += GameplayController_OnPlayerUnitDead;
        Tries++;
    }

}
