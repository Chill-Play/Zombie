using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class BaseLoader : MonoBehaviour
{
    [SerializeField] SceneReference baseLevel;
    [SerializeField] SceneReference tutorialLevel;

    void Start()
    {
        DOTween.Init();
        int levelNumber = ZombiesLevelController.Instance.RaidIsComplited;
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
