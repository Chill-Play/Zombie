using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQBuilding : BaseObject
{
    public event System.Action<int> OnPointAdded;
    public event System.Action OnLevelUp;
    public event System.Action<int> OnRewardOpened;

    [BaseSerialize] int level;
    [SerializeField] int baseCost;
    [SerializeField] float costPower;
    [SerializeField] float costMultiplier;

    [SerializeField] Sprite pointIcon;
    [SerializeField] int cost;
    [BaseSerialize] int currentCount;
    private LevelProgressionController levelProgressionController;
    int nextChest;
    UINumbers uiNumbers;

    public int Level => level;
    public int Cost => cost;
    public int CurrentCount => currentCount;

    public int NextChest => nextChest;
    int rewardCount => levelProgressionController.CurrentLevelProgression.Chests.Count;

    public int RewardCount => rewardCount;

    private void Awake()
    {
        cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
        uiNumbers = FindObjectOfType<UINumbers>();
        float currentValue = (float)currentCount / (float)cost;
        levelProgressionController = FindObjectOfType<LevelProgressionController>();
        nextChest = Mathf.FloorToInt(currentValue / (1f / (rewardCount + 1)));
    }

    private void Start()
    {
        UpdateUnlockables();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddPoint(1);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp();
        }
    }

    public void AddPoint(int value = 1)
    {
        uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "+" + 1, Vector2.zero, 15f, 10f, 0.4f, pointIcon);
        currentCount += value;
        if (currentCount >= cost)
        {
            currentCount = 0;
            LevelUp();
        }

        float currentValue = (float)currentCount / (float)cost;
        if (currentValue >= (nextChest + 1) * (1f / (rewardCount + 1)))
        {
            OnRewardOpened?.Invoke(nextChest);
            nextChest++;
        }
        OnPointAdded?.Invoke(value);
        RequireSave();
    }


    public void LevelUp()
    {
        nextChest = 0;
        level += 1;
        cost = MetaUtils.GetLevelCost(level, costMultiplier, costPower, baseCost);
        RequireSave();
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
