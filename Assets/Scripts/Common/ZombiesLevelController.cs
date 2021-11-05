using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombiesLevelController : SingletonMono<ZombiesLevelController>
{
    [SerializeField] LevelCollection levelCollection;
    [SerializeField] SceneReference baseLevel;

    int levelsPlayed;
    int currentPack;   
    int levelInPack;

    public int LevelsPlayed => levelsPlayed;

    private void Awake()
    {
        LevelService.Instance.Setup(levelCollection);
        var info = LevelService.Instance.CurrentSequenceInfo;
        currentPack = info.pack;
        levelInPack = info.levelInPack;
        levelsPlayed = info.levelsPlayed;
    }

    public void NextRaid()
    {
        SceneReference scene = (levelCollection.GetPack(currentPack) as LevelPack).GetLevel(levelInPack - 1);      
        SceneManager.LoadScene(scene);
    }


    public void RaidStarted()
    {
        LevelService.Instance.LevelStarted();
    }

    public void ToBase()
    {
        SceneManager.LoadScene(baseLevel);
    }

    public void RaidFinished()
    {
        LevelService.Instance.LevelFinished();
    }

    public void RaidFailed()
    {
        LevelService.Instance.LevelFailed(0f);
    }

}
