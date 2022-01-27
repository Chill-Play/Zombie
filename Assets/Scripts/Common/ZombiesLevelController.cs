using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ZombiesLevelController : SingletonMono<ZombiesLevelController>
{
    [SerializeField] LevelCollection raidLevelCollection;
    [SerializeField] LevelCollection campaignLevelCollection;
    [SerializeField] SceneReference baseLevel;

    LevelService raidLevelService;
    LevelService campaignLevelService;

    public int RaidIsCompleted { get; private set; }
    public int StatesConpleted { get; private set; }
    public LevelService RaidLevelService => raidLevelService;
    public LevelService CampaignLevelService => campaignLevelService;

    private void Awake()
    {
        raidLevelService = LevelService.Get(raidLevelCollection);
        campaignLevelService = LevelService.Get(campaignLevelCollection);
        AnalyticsService.Instance.Setup(raidLevelService);
        AnalyticsService.Instance.Setup(campaignLevelService);
        
        RaidIsCompleted = raidLevelService.CurrentSequenceInfo.id;
        StatesConpleted = campaignLevelService.CurrentSequenceInfo.id;
    }

    public void NextRaid()
    {
        DOTween.KillAll();
        LevelSequenceInfo info = raidLevelService.CurrentSequenceInfo;
        SceneReference scene = (raidLevelCollection.GetPack(info.pack) as LevelPack).GetLevel(info.levelInPack);      
        SceneManager.LoadScene(scene);
    }

    public void NextState()
    {
        DOTween.KillAll();
        LevelSequenceInfo info = campaignLevelService.CurrentSequenceInfo;      
        SceneReference scene = (campaignLevelCollection.GetPack(info.pack) as LevelPack).GetLevel(info.levelInPack);
        SceneManager.LoadScene(scene);
    }


    public void RaidStarted()
    {
        raidLevelService.LevelStarted();
    }

    public void CampaignStarted()
    {
        campaignLevelService.LevelStarted();
    }

    public void ToBase()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(baseLevel);
    }

    public void RaidFinished()
    {
        RaidIsCompleted++;
        raidLevelService.LevelFinished();
    }

    public void CampaignFinished()
    {
        StatesConpleted++;
        campaignLevelService.LevelFinished();
    }

    public void RaidFailed()
    {     
        raidLevelService.LevelFailed(0f);
    }

    public void CampaignFailed()
    {
        campaignLevelService.LevelFailed(0f);
    }

    private void OnDisable()
    {
        AnalyticsService.Instance.Unsubscribe(raidLevelService);
        AnalyticsService.Instance.Unsubscribe(campaignLevelService);
    }

}
