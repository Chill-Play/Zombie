using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : SingletonMono<GameplayController>
{
    [SerializeField] CameraController cameraController;
    [SerializeField] Player playerPrefab;
    [SerializeField] InputJoystick InputJoystick;
    public Player playerInstance;


    private void Awake()
    {
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        SpawnPlayer(spawnPoint.transform.position, playerPrefab);
    }



    public void SpawnPlayer(Vector3 point, Player prefab)
    {
        playerInstance = Instantiate(prefab, point, Quaternion.identity);
        cameraController.SetTarget(playerInstance.transform);
        InputJoystick.InputReceiver = playerInstance;
    }

}
