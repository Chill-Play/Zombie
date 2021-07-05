using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    [SerializeField] Button raidButton;
    [SerializeField] LevelPackProgressBar levelBar;
    [SerializeField] ResourcesInfoUIPanel resourcesInfo;


    void Start()
    {
        raidButton.gameObject.SetActive(false);
        levelBar.Setup(LevelController.Instance.CurrentLevel, LevelController.Instance.TotalLevelsInPack);


        raidButton.onClick.AddListener(() => RaidButton_OnClick());
        RaidZone zone = FindObjectOfType<RaidZone>();
        zone.OnEnterZone += Zone_OnEnterZone;
        zone.OnExitZone += Zone_OnExitZone;
        ResourcesController.Instance.OnResourcesUpdated += ResourcesController_OnResourcesUpdated;

        List<ResourceType> resources = ResourcesController.Instance.Resources;
        foreach(var type in resources)
        {
            resourcesInfo.UpdateBar(type, type.Count);
        }
    }

    private void ResourcesController_OnResourcesUpdated()
    {
        List<ResourceType> resources = ResourcesController.Instance.Resources;
        foreach (var type in resources)
        {
            resourcesInfo.UpdateBar(type, type.Count);
        }
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }


    void RaidButton_OnClick()
    {
        Game.Instance.RunRaid();
    }
}
