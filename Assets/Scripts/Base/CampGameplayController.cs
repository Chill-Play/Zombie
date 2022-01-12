using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampGameplayController : SingletonMono<CampGameplayController>
{
    const string LAST_RAID_TYPE_PREFS = "M_Last_Raid_Type";


    public event System.Action<float> OnRaidReadiness;
    public event System.Action OnRaidUnpreparedness;
    public event System.Action OnRunRaid;

    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] InputPanel inputPanel;
    [SerializeField] float timeBeforeRaid = 3f;
    [SerializeField] float timeBeforeCampaign = 3f;
    [SerializeField] RaidZone raidZone;
    [SerializeField] RaidZone campaignZone;

    public Transform PlayerInstance { get;private set; }

    bool isPlayerReturnedToRaidZone = false;

   

    private void Awake()
    {     
        raidZone.OnEnterZone += RaidZone_OnEnterZone; 
        raidZone.OnExitZone += RaidZone_OnExitZone;
        campaignZone.OnEnterZone += CampaignZone_OnEnterZone;
        campaignZone.OnExitZone += CampaignZone_OnExitZone;

        int lastRaidType = PlayerPrefs.GetInt(LAST_RAID_TYPE_PREFS, 0);
        switch (lastRaidType)
        {
            case 0:
                SpawnPlayer(raidZone.SpawnPoint, playerPrefab);
                break;
            case 1:
                SpawnPlayer(campaignZone.SpawnPoint, playerPrefab);
                break;
        }
        
      
    }

    private void CampaignZone_OnExitZone()
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

    private void CampaignZone_OnEnterZone()
    {

        if (isPlayerReturnedToRaidZone)
        {
            OnRaidReadiness?.Invoke(timeBeforeRaid);
            PlayerInstance.GetComponent<UnitMovement>().MoveTo(campaignZone.transform.position);
            StartCoroutine(CampaignCoroutine());
        }
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
            PlayerInstance.GetComponent<UnitMovement>().MoveTo(raidZone.transform.position);
            StartCoroutine(RaidCoroutine());
        }
    }

    IEnumerator RaidCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeRaid);
        OnRunRaid?.Invoke();
        PlayerPrefs.SetInt(LAST_RAID_TYPE_PREFS, 0);
        ZombiesLevelController.Instance.NextRaid();
    }


    IEnumerator CampaignCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeCampaign);
        OnRunRaid?.Invoke();
        PlayerPrefs.SetInt(LAST_RAID_TYPE_PREFS, 1);
        ZombiesLevelController.Instance.NextState();
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
         FindObjectOfType<CampSquad>().SpawnSquad(PlayerInstance.transform.position);
    }



    public void SpawnPlayer(Transform point, GameObject prefab)
    {
        PlayerInstance = Instantiate(prefab, point.position, point.rotation).transform;
        cameraController.SetTarget(PlayerInstance);                
        inputPanel.Receiver = PlayerInstance.GetComponent<IInputReceiver>();
    }

    public void EnableInput()
    {
       
    }

    public void DisableInput()
    {
        
    }
}
