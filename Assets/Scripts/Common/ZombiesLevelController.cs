using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombiesLevelController : SingletonMono<ZombiesLevelController>
{
    [SerializeField] LevelCollection raidLevelCollection;
    [SerializeField] LevelCollection campaignLevelCollection;
    [SerializeField] SceneReference baseLevel;

    LevelService raidLevelService;
    LevelService campaignLevelService;

    public int RaidIsPlayed { get; private set; }
    public int StatesIsPlayed { get; private set; }

    private void Awake()
    {
        raidLevelService = LevelService.Get(raidLevelCollection);
        campaignLevelService = LevelService.Get(campaignLevelCollection);        
        RaidIsPlayed = raidLevelService.CurrentSequenceInfo.levelsPlayed;
        StatesIsPlayed = campaignLevelService.CurrentSequenceInfo.levelsPlayed;
    }

    public void NextRaid()
    {
        LevelSequenceInfo info = raidLevelService.CurrentSequenceInfo;
        SceneReference scene = (raidLevelCollection.GetPack(info.pack) as LevelPack).GetLevel(info.levelInPack);      
        SceneManager.LoadScene(scene);
    }

    public void NextState()
    {
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
        SceneManager.LoadScene(baseLevel);
    }

    public void RaidFinished()
    {
        raidLevelService.LevelFinished();
    }

    public void CampaignFinished()
    {
        campaignLevelService.LevelFinished();
    }

    public void RaidFailed()
    {
        raidLevelService.LevelFailed(0f);
    }

    public void CampaignFailed()
    {
        campaignLevelService.LevelFinished();
    }

}
