using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUI : UIScreen
{
    //[SerializeField] Button raidButton;
    [SerializeField] Transform raidEntrance;
    [SerializeField] TMP_Text raidEntranceTimer;
    //[SerializeField] Button globalMapButton;
    //[SerializeField] LevelPackProgressBar levelBar;
    //[SerializeField] ResourcesInfoUIPanel resourcesInfo;
    //[SerializeField] Bar completionProgressBar;

    CampGameplayController campGameplayController;
    int raidTimer;

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

    private void OnRaidReadiness(float timeBeforeRaid)
    {
        raidEntrance.gameObject.SetActive(true);
        raidEntrance.transform.localScale = Vector3.one * 0.4f;
        raidEntrance.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.2f, 0.3f);
        raidTimer = (int)timeBeforeRaid;
        StartCoroutine(RunRaidTimer());
    }

    private void OnRaidUnpreparedness()
    {
        raidEntrance.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InExpo).OnComplete(() => raidEntrance.gameObject.SetActive(false));
    }

    IEnumerator RunRaidTimer()
    {              
        while (raidTimer > 0)
        {            
            yield return new WaitForSeconds(1f);
            raidTimer--;
            raidEntranceTimer.text = raidTimer.ToString();
            raidEntranceTimer.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);
         
        }
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
