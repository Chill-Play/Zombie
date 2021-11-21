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
        reviveController = FindObjectOfType<ReviveController>();
        reviveController.OnRevive += ReviveController_OnRevive;
    }


    void Start()
    {
        OnLevelStarted?.Invoke();   
    }

    void OnEnable()
    {
        gameplayController = FindObjectOfType<GameplayController>();
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


    public LevelInfo GetLevelInfo()
    {
        float progress = 0f;
        for(int i = 0; i < survivorPickups.Length; i++)
        {
            if (survivorPickups[i] == null)
            {
                progress += 1 / survivorPickups.Length;
            }
        }
        progress = Mathf.Clamp01(progress);
        return new LevelInfo()
        {
            levelsPlayed = LevelController.Instance.LevelsPlayed,
            levelNumber = LevelController.Instance.CurrentLevel,
            levelName = SceneManager.GetActiveScene().name.ToSnakeCase(),
            levelId = LevelController.Instance.LevelId,
            loop = LevelController.Instance.Loop,
            progress = progress,
        };
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
