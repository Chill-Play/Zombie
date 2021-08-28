using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : UIScreen
{
    [SerializeField] Button raidButton;
    //[SerializeField] Button globalMapButton;
    //[SerializeField] LevelPackProgressBar levelBar;
    //[SerializeField] ResourcesInfoUIPanel resourcesInfo;
    //[SerializeField] Bar completionProgressBar;


    private void OnEnable()
    {
        FindObjectOfType<RaidZone>().OnEnterZone += Zone_OnEnterZone;
    }

    void Start()
    {
       raidButton.gameObject.SetActive(false);
       raidButton.onClick.AddListener(() => RaidButton_OnClick());
    }

    private void OnDisable()
    {
        var zone = FindObjectOfType<RaidZone>();
        if(zone != null)
        {
            zone.OnExitZone += Zone_OnExitZone;
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

    private void Zone_OnExitZone()
    {       
       raidButton.transform.DOScale(0.5f, 0.15f).SetEase(Ease.InExpo).OnComplete(() => raidButton.gameObject.SetActive(false));
    }

    private void Zone_OnEnterZone()
    {
       raidButton.gameObject.SetActive(true);
       raidButton.transform.localScale = Vector3.one * 0.4f;
       raidButton.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic, 1.2f, 0.3f);
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
