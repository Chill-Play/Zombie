using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampGameplayController : SingletonMono<CampGameplayController>
{
    public event System.Action<float> OnRaidReadiness;
    public event System.Action OnRaidUnpreparedness;
    public event System.Action OnRunRaid;

    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputPanel inputPanel;
    [SerializeField] float timeBeforeRaid = 3f;

    [HideInInspector] public Transform playerInstance;

    bool isPlayerReturnedToRaidZone = false;

    RaidZone raidZone;

    private void Awake()
    {
        raidZone = FindObjectOfType<RaidZone>(true);
        raidZone.OnEnterZone += RaidZone_OnEnterZone; 
        raidZone.OnExitZone += RaidZone_OnExitZone;

        Vector3 spawnPos = Vector3.zero;
        if (raidZone != null)
        {
            spawnPos = raidZone.transform.position;
        }
        SpawnPlayer(spawnPos, playerPrefab);
    }

    public void SetPlayerReturnedToRaidZone(bool value)
    {
        isPlayerReturnedToRaidZone = value;
    }

    private void RaidZone_OnEnterZone()
    {
        if (isPlayerReturnedToRaidZone)
        {
            OnRaidReadiness?.Invoke(timeBeforeRaid);
            playerInstance.GetComponent<UnitMovement>().MoveTo(raidZone.transform.position);
            StartCoroutine(RaidCoroutine());
        }
    }

    IEnumerator RaidCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeRaid);
        OnRunRaid?.Invoke();
        ZombiesLevelController.Instance.NextRaid();
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
        //InputJoystick.InputReceiver = playerInstance.GetComponent<IInputReceiver>();           
        inputPanel.Receiver = playerInstance.GetComponent<IInputReceiver>();
    }
}
