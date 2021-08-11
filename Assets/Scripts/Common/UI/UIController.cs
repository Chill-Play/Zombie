using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : SingletonMono<UIController>
{
    [SerializeField] List<UIScreen> screens;
    UIScreen activeScreen;

    void Update()
    {

    }


    public void ShowScreen(SubjectId screenId)
    {
        UIScreen screen = screens.FirstOrDefault((x) => x.Id == screenId);
        if(screen != null)
        {
            if(activeScreen != null)
            {
                HideScreen(activeScreen);
            }
            screen.gameObject.SetActive(true);
            activeScreen = screen;
        }
        else
        {
            Debug.LogError("No screen found with id : " + screenId.name);
        }
    }


    void HideScreen(UIScreen screen)
    {
        screen.gameObject.SetActive(false);
    }
}
