using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorialContent : MonoBehaviour
{
    [SerializeField] List<GameObject> tutorialContent = new List<GameObject>();
    [SerializeField] TutorialText tutorialUIInfo;
    [SerializeField] ResourcesInfo startResources;
    [SerializeField] private RaidZone raidZone;

    const string TUTORIAL_COMPLETED_KEY = "tutorial_completed";
    bool completed;

    private void Awake()
    { 
        completed = PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 1;
        // completed = false;
        if (completed)
        {
            for (int i = 0; i < tutorialContent.Count; i++)
            {
                Destroy(tutorialContent[i]);
            }
        }
        else
        {
            var tutorialTexts = FindObjectsOfType<TutorialText>(true);
            for (int i = 0; i < tutorialTexts.Length; i++)
            {
                tutorialTexts[i].Setup();
            }
            PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, 1);
            raidZone.gameObject.SetActive(false);  
        }
    }

    private void Start()
    {
        if (!completed)
        {
            ResourcesController resourcesController = ResourcesController.Instance;
            resourcesController.AddResources(startResources);
            resourcesController.UpdateResources();   
        }
    }
}
