using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUI : UIScreen
{
    //[SerializeField] Button raidButton;
    [SerializeField] RaidEntranceUI raidEntrance;
    [SerializeField] RaidEntranceUI campaignEntrance;
    [SerializeField] private LevelUpScreen levelUpScreen;
    private HQBuilding hq;

    //[SerializeField] Button globalMapButton;
    //[SerializeField] LevelPackProgressBar levelBar;
    //[SerializeField] ResourcesInfoUIPanel resourcesInfo;
    //[SerializeField] Bar completionProgressBar;

    CampGameplayController campGameplayController;
    int raidTimer;

    private void Awake()
    {
        hq = FindObjectOfType<HQBuilding>();
    }

    private void OnEnable()
    {
        CampGameplayController campGameplayController = CampGameplayController.Instance;
        hq.OnLevelUp += levelUpScreen.ShowScreen;
        campGameplayController.OnRaidReadiness += OnRaidReadiness;
        campGameplayController.OnRaidUnpreparedness += OnRaidUnpreparedness;
        campGameplayController.OnCampaignReadiness += CampGameplayController_OnCampaignReadiness; ;
        campGameplayController.OnCampaignUnpreparedness += CampGameplayController_OnCampaignUnpreparedness; ;
    }

    private void CampGameplayController_OnCampaignUnpreparedness()
    {
        campaignEntrance.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InExpo).OnComplete(() => raidEntrance.gameObject.SetActive(false));
    }

    private void CampGameplayController_OnCampaignReadiness(float timeBeforeRaid)
    {
        campaignEntrance.gameObject.SetActive(true);
        campaignEntrance.transform.localScale = Vector3.one * 0.4f;
        campaignEntrance.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.2f, 0.3f);
        campaignEntrance.RunTimer(timeBeforeRaid);
    }

    void Start()
    {       
        //raidButton.gameObject.SetActive(false);
        //raidButton.onClick.AddListener(() => RaidButton_OnClick());

    }

    private void OnDisable()
    {
        if (hq != null)
            hq.OnLevelUp -= levelUpScreen.ShowScreen;
        if (campGameplayController != null)
        {
            campGameplayController.OnRaidReadiness -= OnRaidReadiness;           
            campGameplayController.OnRaidUnpreparedness -= OnRaidUnpreparedness;
        }
    }

    //private void MapController_OnCompletionProgressUpdate(float value)
    //    {
    //        completionProgressBar.SetValue(value);
    //    }

    //private void GlobalMapArea_OnGlobalMapAreaExit()
    //{
    //    globalMapButton.gameObject.SetActive(false);
    //}

    //private void GlobalMapArea_OnGlobalMapAreaEnter()
    //{
    //    globalMapButton.gameObject.SetActive(true);
    //}

    //private void ResourcesController_OnResourcesUpdated()
    //{
    //    List<ResourceType> resources = ResourcesController.Instance.Resources;
    //    foreach (var type in resources)
    //    {
    //        resourcesInfo.UpdateBar(type, type.Count);
    //    }
    //}

    private void OnRaidReadiness(float timeBeforeRaid)
    {
        raidEntrance.gameObject.SetActive(true);
        raidEntrance.transform.localScale = Vector3.one * 0.4f;
        raidEntrance.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.2f, 0.3f);
        raidEntrance.RunTimer(timeBeforeRaid);
    }

    private void OnRaidUnpreparedness()
    {
        raidEntrance.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InExpo).OnComplete(() => raidEntrance.gameObject.SetActive(false));
    }


    //// Update is called once per frame
    //void Update()
    //{

    //}


    //void GlobalMap_OnClick()
    //{
    //    Game.Instance.ToGlobalMap();
    //}
}
