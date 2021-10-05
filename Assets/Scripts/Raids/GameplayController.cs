using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : SingletonMono<GameplayController>
{
    public event System.Action OnReturnedToBase;
    public event System.Action OnPlayerUnitDead;

    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputJoystick InputJoystick;

    public Squad SquadInstance { get; set; }

    SpawnPoint spawnPoint;
    ReviveController reviveController;

    private void Awake()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);

        Level level = FindObjectOfType<Level>();
        spawnPoint.OnReturnedToBase += SpawnPoint_OnReturnedToBase;
        level.OnLevelFailed += OnLevelFailed;
        reviveController = FindObjectOfType<ReviveController>();
        reviveController.OnRevive += Level_OnRevive;
        SquadInstance.OnPlayerUnitDead += SquadInstance_OnPlayerUnitDead;
    }

    private void SquadInstance_OnPlayerUnitDead()
    {
        OnPlayerUnitDead?.Invoke();      
    }

    private void Level_OnRevive()
    {
        InputJoystick.InputReceiver = SquadInstance;
    }

    private void SpawnPoint_OnReturnedToBase()
    {
        spawnPoint.OnReturnedToBase -= SpawnPoint_OnReturnedToBase;
        InputJoystick.InputReceiver = null;
        SquadInstance.MoveToCar(spawnPoint, InCar);
       
    }

    public void InCar()
    {
        OnReturnedToBase?.Invoke();
    }

    public void SpawnPlayer(Vector3 point, GameObject prefab)
    {      
        Squad squad = FindObjectOfType<Squad>();
        if (squad != null)
        {
            cameraController.SetTarget(squad.transform);
            InputJoystick.InputReceiver = squad.GetComponent<IInputReceiver>();
            SquadInstance = squad;
        }
    }

    private void OnLevelFailed()
    {
        InputJoystick.InputReceiver = null;      
    }

}
