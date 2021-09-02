using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishedTrigger : ConditionTrigger
{
    private void Awake()
    {
        FindObjectOfType<Level>().OnLevelEnded += Level_OnLevelEnded;
    }

    private void Level_OnLevelEnded()
    {
        InvokeEvent();
    }
}
