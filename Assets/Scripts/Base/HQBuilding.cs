using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQBuilding : BaseObject
{
    public event System.Action OnLevelUp;

    [BaseSerialize] int level;
    [SerializeField] ResourcesInfo baseCost;
    [SerializeField] float costPower;
    [SerializeField] float costMultiplier;

    public int Level => level;


    private void Start()
    {
        UpdateUnlockables();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp();
        }
    }


    public void LevelUp()
    {
        level += 1;
        RequireSave();
        UpdateUnlockables();
        OnLevelUp?.Invoke();
        UnityAnalytics.Instance.OnHQLevelUp(level);
    }


    public ResourcesInfo GetCostForLevelUp()
    {
        return MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
    }


    private void UpdateUnlockables()
    {
        var unlockables = FindObjectsOfType<UnlockableBuilding>();
        foreach (var unlockable in unlockables)
        {
            unlockable.SetLevel(level);
        }
    }

}
