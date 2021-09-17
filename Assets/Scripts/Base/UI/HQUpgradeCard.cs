using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HQUpgradeCard : UpgradeCard
{
    [SerializeField] TMP_Text levelLabel; 
    public void Setup(int level, ResourcesInfo cost, ResourcesInfo resources, System.Action OnClick)
    {
        Setup(cost, resources, OnClick);
        levelLabel.text = "LVL " + level;
    }
}
