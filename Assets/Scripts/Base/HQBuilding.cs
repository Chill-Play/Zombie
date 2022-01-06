using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQBuilding : BaseObject
{
    public event System.Action<int> OnPointAdded;
    public event System.Action OnLevelUp;

    [BaseSerialize] int level;
    [SerializeField] int baseCost;
    [SerializeField] float costPower;
    [SerializeField] float costMultiplier;

    [SerializeField] int cost;
    [BaseSerialize] int currentCount;

    public int Level => level;
    public int Cost => cost;
    public int CurrentCount => currentCount;


    private void Awake()
    {
        cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
    }

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

    public void AddPoint(int value = 1)
    {
        currentCount += value;
        if (currentCount >= cost)
        {
            currentCount = 0;
            LevelUp();
        }
        OnPointAdded?.Invoke(value);
        RequireSave();
    }


    public void LevelUp()
    {
        level += 1;
        cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
        RequireSave();
        UpdateUnlockables();
        OnLevelUp?.Invoke();
        UnityAnalytics.Instance.OnHQLevelUp(level);
    }


    public ResourcesInfo GetCostForLevelUp()
    {
        return new ResourcesInfo();//MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
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
