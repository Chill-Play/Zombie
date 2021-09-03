using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : UIScreen
{
    //[SerializeField] Button raidButton;
    [SerializeField] Transform raidEntrance;
    //[SerializeField] Button globalMapButton;
    //[SerializeField] LevelPackProgressBar levelBar;
    //[SerializeField] ResourcesInfoUIPanel resourcesInfo;
    //[SerializeField] Bar completionProgressBar;

    CampGameplayController campGameplayController;

    private void OnEnable()
    {
       
    }

   

    void Start()
    {       
        //raidButton.gameObject.SetActive(false);
        //raidButton.onClick.AddListener(() => RaidButton_OnClick());

        CampGameplayController campGameplayController = CampGameplayController.Instance;

        campGameplayController.OnRaidReadiness += OnRaidReadiness;
        campGameplayController.OnRaidUnpreparedness += OnRaidUnpreparedness;
    }

    private void OnDisable()
    {
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

    private void OnRaidReadiness()
    {
        raidEntrance.gameObject.SetActive(true);
        raidEntrance.transform.localScale = Vector3.one * 0.4f;
        raidEntrance.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.2f, 0.3f);
    }

    private void OnRaidUnpreparedness()
    {
        raidEntrance.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InExpo).OnComplete(() => raidEntrance.gameObject.SetActive(false));
    }


    //// Update is called once per frame
    //void Update()
    //{

    //}


    void RaidButton_OnClick()
    {
       Game.Instance.RunRaid();
    }

    //void GlobalMap_OnClick()
    //{
    //    Game.Instance.ToGlobalMap();
    //}
}
