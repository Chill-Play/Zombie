using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : SingletonMono<GameplayController>
{
    public event System.Action OnReturnedToBase;
    public event System.Action OnPlayerUnitDead;

    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputPanel inputPanel;

    public Squad SquadInstance { get; set; }

    SpawnPoint spawnPoint;
    ReviveController reviveController;

    private void Awake()
    {
        spawnPoint = SpawnPoint.Instance;
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);

        Level level = Level.Instance;
        spawnPoint.OnReturnedToBase += SpawnPoint_OnReturnedToBase;
        level.OnLevelFailed += OnLevelFailed;
        reviveController = ReviveController.Instance;
        reviveController.OnRevive += Level_OnRevive;
        SquadInstance.OnPlayerUnitDead += SquadInstance_OnPlayerUnitDead;
    }

    private void SquadInstance_OnPlayerUnitDead()
    {
        OnPlayerUnitDead?.Invoke();      
    }

    private void Level_OnRevive()
    {
        inputPanel.EnableInput();
    }

    private void SpawnPoint_OnReturnedToBase()
    {
        spawnPoint.OnReturnedToBase -= SpawnPoint_OnReturnedToBase;
        inputPanel.DisableInput();
        SquadInstance.MoveToCar(spawnPoint, InCar);
       
    }

    public void InCar()
    {
        OnReturnedToBase?.Invoke();
    }

    public void SpawnPlayer(Vector3 point, GameObject prefab)
    {      
        Squad squad = Squad.Instance;
        if (squad != null)
        {
            cameraController.SetTarget(squad.transform);
            inputPanel.Receiver = squad.GetComponent<IInputReceiver>();
            SquadInstance = squad;
        }
    }

    private void OnLevelFailed()
    {
        inputPanel.DisableInput();
    }

}
