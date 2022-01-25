using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class UIController : SingletonMono<UIController>
{
    [SerializeField] List<UIScreen> screens;
    static SubjectId activeScreen;

    public void ReturnToDefaultScreen()
    {
        ShowScreen(screens[0].Id);
    }


    public UIScreen ShowScreen(SubjectId screenId)
    {
        //FindObjectOfType<InputJoystick>().ResetInput();
        UIScreen screen = screens.FirstOrDefault((x) => x.Id == screenId);
        if (screen != null)
        {
            if (activeScreen != null)
            {
                HideScreen(activeScreen);
                //UnityAnalytics.Instance.OnScreenSwitched(activeScreen.name);
            }
            else
            {
               // UnityAnalytics.Instance.OnScreenSwitched(screenId.name);
            }
            screen.gameObject.SetActive(true);
            activeScreen = screen.Id;
        }
        else
        {
            Debug.LogError("No screen found with id : " + screenId.name);
        }
        return screen;
    }


    void HideScreen(SubjectId id)
    {
        var screen = GetScreenById(id);
        if (screen != null)
        {
            screen.gameObject.SetActive(false);
        }
    }


    UIScreen GetScreenById(SubjectId id)
    {
        return screens.FirstOrDefault((x) => x.Id == id);
    }


}
