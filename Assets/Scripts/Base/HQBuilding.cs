using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQBuilding : BaseObject
{
    [BaseSerialize] int level;

    public int Level => level;


    private void Start()
    {
        FindObjectOfType<UnlockableBuilding>().SetLevel(level);
    }


    public void LevelUp()
    {
        level += 1;
        RequireSave();
        FindObjectOfType<UnlockableBuilding>().SetLevel(level);
    }
}
