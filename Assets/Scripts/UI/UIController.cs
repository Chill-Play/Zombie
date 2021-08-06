using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : SingletonMono<UIController>
{
    //[SerializeField] BaseUI baseUI;
    [SerializeField] InGameUI inGame;
    [SerializeField] RaidFinishScreen finishScreen;
    [SerializeField] FailedUI failedUI;

    // Start is called before the first frame update
    void Awake()
    {
        LevelBase levelBase = FindObjectOfType<LevelBase>();
        //if(baseUI == null)
        {
            return;
        }
        if(levelBase != null)
        {
            //baseUI.gameObject.SetActive(true);
            inGame.gameObject.SetActive(false);
        }
        else
        {
            //baseUI.gameObject.SetActive(false);
            inGame.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowFinishScreen(Dictionary<ResourceType, int> resources)
    {
        inGame.gameObject.SetActive(false);
        finishScreen.gameObject.SetActive(true);
        finishScreen.Show(resources);
    }


    public void ShowFailedScreen()
    {
        inGame.gameObject.SetActive(false);
        failedUI.gameObject.SetActive(true);
    }
}
