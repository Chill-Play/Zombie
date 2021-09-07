using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelController : SingletonMono<LevelController>
{
    public const string LEVEL_NUMBER_PREFS = "G_LevelNumber";
    public const string PREF_CURRENT_LEVEL = "G_CurrentLevel";
    const string PREF_CURRENT_PACK = "G_CurrentPack";
    

    [SerializeField] SceneReference baseLevel;
    [SerializeField] SceneReference gloabalMap;
    [SerializeField] LevelCollection collection;

    int currentPack;
    int currentLevel;
    int totalLevelsInPack;


    public int CurrentLevel => currentLevel;
    public int TotalLevelsInPack => totalLevelsInPack;
    public int LevelId => currentLevel % totalLevelsInPack;
    public int Loop => (currentLevel / totalLevelsInPack) + 1;


    private void Awake()
    {
        currentPack = PlayerPrefs.GetInt(PREF_CURRENT_PACK, 0);
        currentLevel = PlayerPrefs.GetInt(PREF_CURRENT_LEVEL, 0);
        totalLevelsInPack = collection.GetPack(currentPack).GetLevelsCount();
    }


    public void NextRaid()
    {
        SceneReference scene = collection.GetPack(currentPack).GetLevel(currentLevel - 1);
        Debug.Log("Pack : " + currentPack + " Level : " + currentLevel);
        SceneManager.LoadScene(scene);
    }


    public void ToBase(bool finished)
    {
        if (finished)
        {
            RaidFinished();
        }
        SceneManager.LoadScene(baseLevel);
    }

    public void ToGlobalMap()
    {
        SceneManager.LoadScene(gloabalMap);
    }


    public void RaidFinished()
    {
        LevelPack pack = collection.GetPack(currentPack);
        currentLevel++;
        PlayerPrefs.SetInt(PREF_CURRENT_PACK, currentPack);
        PlayerPrefs.SetInt(PREF_CURRENT_LEVEL, currentLevel);
    }

}
