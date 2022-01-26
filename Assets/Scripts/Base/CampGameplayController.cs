using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampGameplayController : SingletonMono<CampGameplayController>
{
    const string LAST_RAID_TYPE_PREFS = "M_Last_Raid_Type";


    public event System.Action<float> OnRaidReadiness;
    public event System.Action OnRaidUnpreparedness;
    public event System.Action<float> OnCampaignReadiness;
    public event System.Action OnCampaignUnpreparedness;
    public event System.Action OnRunRaid;

    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<int> campaignHQLevel = new List<int>();
    [SerializeField] InputPanel inputPanel;
    [SerializeField] float timeBeforeRaid = 3f;
    [SerializeField] float timeBeforeCampaign = 3f;
    [SerializeField] RaidZone raidZone;
    [SerializeField] CampaignZone campaignZone;

    public Transform PlayerInstance { get;private set; }

    bool isPlayerReturnedToRaidZone = false;
    HQBuilding hq;
    ZombiesLevelController zombiesLevelController;
    bool campaignAllowed = false;




    private void Awake()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnLevelUp += Hq_OnLevelUp;
        zombiesLevelController = FindObjectOfType<ZombiesLevelController>();
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

    private void Hq_OnLevelUp(int level)
    {
        SetCampaignZoneStatus();
    }

    void SetCampaignZoneStatus()
    {
        int levelPlayed = zombiesLevelController.StatesConplited;
        int lvlNedded = -1;
        if (levelPlayed < campaignHQLevel.Count)
            lvlNedded = campaignHQLevel[levelPlayed];
        else
            lvlNedded = campaignHQLevel[campaignHQLevel.Count - 1];
        campaignAllowed = lvlNedded <= hq.Level;
        campaignZone.SetLevel(lvlNedded, campaignAllowed);
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
        if (isPlayerReturnedToRaidZone && campaignAllowed)
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
         SetCampaignZoneStatus();
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
