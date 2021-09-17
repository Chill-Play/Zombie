using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HQUpgradeCard : UpgradeCard
{
    [SerializeField] TMP_Text levelLabel; 
    public void Setup(int level, ResourcesInfo cost, ResourcesInfo resources, bool freeOption, System.Action<bool> OnClick)
    {
        Setup(cost, resources, freeOption, OnClick);
        levelLabel.text = "LVL " + level;
    }
}
