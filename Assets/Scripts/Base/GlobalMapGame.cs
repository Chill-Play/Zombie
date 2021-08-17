using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapGame : SingletonMono<GlobalMapGame>
{
    [SerializeField] LevelSequence levelSequence;
    [SerializeField] GlobalMapPlayer playerPrefab; 

    StateArea[] stateAreas;
    int levelNumber;
    GlobalMapPlayer player;

    private void Awake()
    {
        
        stateAreas = FindObjectsOfType<StateArea>();
        levelNumber = PlayerPrefs.GetInt(LevelController.LEVEL_NUMBER_PREFS, 0);
        StateArea currentState = GetStateArea(levelSequence.GetScene(levelNumber));      
        player = Instantiate<GlobalMapPlayer>(playerPrefab, currentState.transform.position, Quaternion.identity);
        CameraController.Instance.SetTarget(player.transform);
        CameraController.Instance.Zoom(-9f, 1f);
        //GlobalMapUI.Instance.SetDark(0f, 1f);
    }

    public void MoveToNextState()
    {
        StateArea nextState = GetStateArea(levelSequence.GetScene(levelNumber + 1));
        player.Move(nextState.transform.position, OnReadyToLoadNextState);
    }

    public void OnReadyToLoadNextState()
    {
        PlayerPrefs.SetInt(LevelController.LEVEL_NUMBER_PREFS, levelNumber + 1);
        StateArea nextState = GetStateArea(levelSequence.GetScene(levelNumber + 1));
        //GlobalMapUI.Instance.SetDark(1f, 1f);
        CameraController.Instance.Zoom(-3f, 1f,() => StatesLoader.Instance.LoadState(nextState.StateBase));        
    }

    StateArea GetStateArea(SceneReference sceneReference)
    {
        for (int i = 0; i < stateAreas.Length; i++)
        {
            if (stateAreas[i].StateBase.ScenePath == sceneReference.ScenePath)
            {
                return stateAreas[i];
            }
        }
        return null;
    }

}
