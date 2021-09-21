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
    [SerializeField] GameObject freeLabel;
    [SerializeField] GameObject resourcesGroup;


    public void Setup(StatInfo statInfo, StatsType type, ResourcesInfo resources, bool freeOption, System.Action<bool> OnClick)
    {
        var cost = type.GetLevelCost(statInfo.level);
        Setup(cost, resources, freeOption, () => OnClick?.Invoke(freeOption));
        if (cost.IsFilled(resources) || freeOption)
        {
            icon.sprite = type.icon;
        }
        else
        {
            icon.sprite = type.lockedIcon;
        }
        nameLabel.text = type.displayName;
        levelLabel.text = "LVL " + (statInfo.level + 1);
        freeLabel.gameObject.SetActive(freeOption);
        resourcesGroup.gameObject.SetActive(!freeOption);
    }
}
