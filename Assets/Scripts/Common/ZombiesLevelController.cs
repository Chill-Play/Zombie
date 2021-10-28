using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombiesLevelController : SingletonMono<ZombiesLevelController>
{
    [SerializeField] SceneReference baseLevel;

    public void RaidStarted()
    {
        LevelService.Instance.LevelStarted();
    }

    public void ToBase(bool finished)
    {
        if (finished)
        {
            RaidFinished();
        }
        SceneManager.LoadScene(baseLevel);
    }

    public void RaidFinished()
    {
        LevelService.Instance.LevelFinished();
    }

}
