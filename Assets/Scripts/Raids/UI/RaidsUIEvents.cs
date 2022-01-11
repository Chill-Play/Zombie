using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidsUIEvents : MonoBehaviour
{
    [SerializeField] SubjectId inGameScreenId;
    [SerializeField] SubjectId finishScreenId;
    [SerializeField] SubjectId failedScreenId;
    [SerializeField] SubjectId reviveScreenId;
    [SerializeField] UIController uiController;
    
    // Start is called before the first frame update
    void Start()
    {
        Level level = FindObjectOfType<Level>();
        ReviveController reviveController = FindObjectOfType<ReviveController>();
        level.OnLevelEnded += Level_OnLevelEnded;
        level.OnLevelFailed += Level_OnLevelFailed;
        if (reviveController != null)
        {
            reviveController.OnRevive += Level_OnRevive;
        }
        uiController.ShowScreen(inGameScreenId);
    }

    private void Level_OnRevive()
    {
        uiController.ShowScreen(inGameScreenId);
    }

    private void Level_OnLevelFailed()
    {
        /* if (AdvertisementManager.Instance.RewardedAvailable)
         {
             var screen = (ReviveScreen)uiController.ShowScreen(reviveScreenId);
             screen.OnTimerEnd += () => uiController.ShowScreen(failedScreenId);
         }
         else
         {
             uiController.ShowScreen(failedScreenId);
         }*/
        var screen = (ReviveScreen)uiController.ShowScreen(reviveScreenId);
        screen.OnTimerEnd += () => uiController.ShowScreen(failedScreenId);
    }

    private void Level_OnLevelEnded()
    {
        uiController.ShowScreen(finishScreenId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
