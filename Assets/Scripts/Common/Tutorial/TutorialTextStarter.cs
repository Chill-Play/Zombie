using System;
using UnityEngine;


public class TutorialTextStarter : MonoBehaviour
{
    [SerializeField] private TutorialText[] tutorialTexts;

    private void Start()
    {
        foreach (var tutorialText in tutorialTexts)
            tutorialText.Setup();
    }
}