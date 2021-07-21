using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BaseLoader : MonoBehaviour
{
    [SerializeField] LevelSequence levelSequence;

    void Start()
    {
        int levelNumber = PlayerPrefs.GetInt(LevelController.LEVEL_NUMBER_PREFS, 0);
        SceneReference scene = levelSequence.GetScene(levelNumber);
        SceneManager.LoadScene(scene);
    }
}
