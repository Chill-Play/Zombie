using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeCard : UpgradeCard
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] TMP_Text levelLabel;


    public void Setup(StatInfo statInfo, StatsType type, ResourcesInfo resources, System.Action OnClick)
    {
        var cost = type.GetLevelCost(statInfo.level);
        Setup(cost, resources, OnClick);
        if (cost.IsFilled(resources))
        {
            icon.sprite = type.icon;
        }
        else
        {
            icon.sprite = type.lockedIcon;
        }
        nameLabel.text = type.displayName;
        levelLabel.text = "LVL " + statInfo.level;
    }
}
