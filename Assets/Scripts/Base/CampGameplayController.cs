using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampGameplayController : SingletonMono<CampGameplayController>
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputJoystick InputJoystick;

    [HideInInspector] public Transform playerInstance;
    private void Awake()
    {
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        Vector3 spawnPos = Vector3.zero;
        if (spawnPoint != null)
        {
            spawnPos = spawnPoint.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);
       
    }

    private void Start()
    {
         FindObjectOfType<CampSquad>().SpawnSquad(playerInstance.transform.position);
    }



    public void SpawnPlayer(Vector3 point, GameObject prefab)
    {
        playerInstance = Instantiate(prefab, point, Quaternion.identity).transform;
        cameraController.SetTarget(playerInstance);
        InputJoystick.InputReceiver = playerInstance.GetComponent<IInputReceiver>();           
        
    }
}
