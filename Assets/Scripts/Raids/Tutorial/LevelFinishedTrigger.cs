using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishedTrigger : ConditionTrigger
{
    private void Awake()
    {
        Level.Instance.OnLevelEnded += Level_OnLevelEnded;
    }

    private void Level_OnLevelEnded()
    {
        InvokeEvent();
    }
}
