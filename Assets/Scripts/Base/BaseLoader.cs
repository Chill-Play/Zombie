using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BaseLoader : MonoBehaviour
{
    [SerializeField] SceneReference baseLevel;
    [SerializeField] SceneReference tutorialLevel;

    void Start()
    {
        int levelNumber = PlayerPrefs.GetInt(LevelController.PREF_CURRENT_LEVEL, 0);
        if (levelNumber == 0)
        {
            SceneManager.LoadScene(tutorialLevel);
        }
        else
        {
            SceneManager.LoadScene(baseLevel);
        }
    }
}
