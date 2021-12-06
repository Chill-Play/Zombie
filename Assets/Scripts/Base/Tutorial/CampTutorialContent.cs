using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorialContent : MonoBehaviour
{
    [SerializeField] List<GameObject> tutorialContent = new List<GameObject>();
    [SerializeField] TutorialText tutorialUIInfo;
    [SerializeField] ResourcesInfo startResources;

    const string TUTORIAL_COMPLETED_KEY = "tutorial_completed";
    bool completed;

    private void Awake()
    {
        completed = PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 1) == 1;
        if (completed)
        {
            for (int i = 0; i < tutorialContent.Count; i++)
            {
                Destroy(tutorialContent[i]);
            }
        }
        else
        {
            PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, 1);
            FindObjectOfType<RaidZone>().gameObject.SetActive(false);          
            tutorialUIInfo.gameObject.SetActive(true);
            tutorialUIInfo.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (!completed)
        {
            ResourcesController resourcesController = FindObjectOfType<ResourcesController>();
            resourcesController.AddResources(startResources);
            resourcesController.UpdateResources();   
        }
    }
}
