using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidsUIEvents : MonoBehaviour
{
    [SerializeField] SubjectId inGameScreenId;
    [SerializeField] SubjectId finishScreenId;
    [SerializeField] UIController uiController;
    
    // Start is called before the first frame update
    void Start()
    {
        Level level = FindObjectOfType<Level>();
        level.OnLevelEnded += Level_OnLevelEnded;
        uiController.ShowScreen(inGameScreenId);
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
