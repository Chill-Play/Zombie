using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampGameplayController : SingletonMono<CampGameplayController>
{
    public event System.Action OnRaidReadiness;
    public event System.Action OnRaidUnpreparedness;

    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputJoystick InputJoystick;

    [HideInInspector] public Transform playerInstance;

    bool isPlayerReturnedToRaidZone = false;

    private void Awake()
    {
        RaidZone raidZone = FindObjectOfType<RaidZone>();
        raidZone.OnEnterZone += RaidZone_OnEnterZone; 
        raidZone.OnExitZone += RaidZone_OnExitZone;

        Vector3 spawnPos = Vector3.zero;
        if (raidZone != null)
        {
            spawnPos = raidZone.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);
    }

    private void RaidZone_OnEnterZone()
    {
        if (isPlayerReturnedToRaidZone)
        {
            OnRaidReadiness?.Invoke();
        }
    }

    private void RaidZone_OnExitZone()
    {
        if (!isPlayerReturnedToRaidZone)
        {
            isPlayerReturnedToRaidZone = true;
        }
        else
        {
            OnRaidUnpreparedness?.Invoke();
        }       
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
