using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class BaseLoader : MonoBehaviour
{
    const string RAID_TUTORIAL_PREFS = "M_Raid_Tutorial_Needed";

    [SerializeField] SceneReference baseLevel;
    [SerializeField] SceneReference tutorialLevel;

   

    void Start()
    {
        DOTween.Init();
        bool tutorialNeeded = PlayerPrefs.GetInt(RAID_TUTORIAL_PREFS, 0) == 1;       
        if (!tutorialNeeded)
        {
            PlayerPrefs.SetInt(RAID_TUTORIAL_PREFS, 1);
            SceneManager.LoadScene(tutorialLevel);
        }
        else
        {
            SceneManager.LoadScene(baseLevel);
        }
       
    }
}
