using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CampaignZone : RaidZone
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] Color normalColor;
    [SerializeField] Color closedColor;
    [SerializeField] string additionalText;

    public void SetLevel(int level, bool opened)
    {
        levelText.text = additionalText + (level + 1).ToString();
        levelText.color = opened ? normalColor : closedColor;
    }
}
