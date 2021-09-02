using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorialContent : MonoBehaviour
{
    [SerializeField] List<GameObject> tutorialContent = new List<GameObject>();

    const string TUTORIAL_COMPLITED_KEY = "tutorial_complited";
    bool complited;

    private void Awake()
    {
        complited = PlayerPrefs.GetInt(TUTORIAL_COMPLITED_KEY, 0) == 1;
        if (complited)
        {
            for (int i = 0; i < tutorialContent.Count; i++)
            {
                Destroy(tutorialContent[i]);
            }
        }
        else
        {
            PlayerPrefs.SetInt(TUTORIAL_COMPLITED_KEY, 1);
        }
    }
}
