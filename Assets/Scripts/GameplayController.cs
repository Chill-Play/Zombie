using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : SingletonMono<GameplayController>
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputJoystick InputJoystick;

    public Squad SquadInstance { get; set; }


    private void Awake()
    {
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        Vector3 spawnPos = Vector3.zero;
        if(spawnPoint != null)
        {
            spawnPos = spawnPoint.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);
    }



    public void SpawnPlayer(Vector3 point, GameObject prefab)
    {
        //playerInstance = Instantiate(prefab, point, Quaternion.identity);
        Squad squad = FindObjectOfType<Squad>();
        if (squad != null)
        {
            cameraController.SetTarget(squad.transform);
            InputJoystick.InputReceiver = squad.GetComponent<IInputReceiver>();
            SquadInstance = squad;
        }
    }

}
