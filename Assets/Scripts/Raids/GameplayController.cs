using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : SingletonMono<GameplayController>
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputJoystick InputJoystick;

    public Squad SquadInstance { get; set; }

    Vector3 spawnPos;

    private void Awake()
    {
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        spawnPos = Vector3.zero;
        if(spawnPoint != null)
        {
            spawnPos = spawnPoint.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);

        Level level = FindObjectOfType<Level>();
        level.OnLevelEnded += OnLevelEnded;
        level.OnLevelFailed += OnLevelFailed;
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

    private void OnLevelEnded()
    {
        InputJoystick.InputReceiver = null;
        SquadInstance.GoToPosition(spawnPos);
    }

    private void OnLevelFailed()
    {
        InputJoystick.InputReceiver = null;      
    }

}
