using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image background;
    [SerializeField] Sprite unlockedSprite;
    [SerializeField] Sprite lockedSpite;
    [SerializeField] List<UIResourceSlot> resourceSlots;
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] TMP_Text levelLabel;


    public void Setup(StatInfo statInfo, StatsType type, ResourcesInfo resources)
    {
        var cost = type.levelUpCosts[statInfo.level];
        if(cost.IsFilled(resources))
        {
            background.sprite = unlockedSprite;
            icon.sprite = type.icon;
        }
        else
        {
            background.sprite = lockedSpite;
            icon.sprite = type.lockedIcon;
        }
        for(int i = 0; i < resourceSlots.Count; i++)
        {
            var slot = resourceSlots[i];
            if (i < cost.Slots.Count)
            {
                slot.gameObject.SetActive(true);
                slot.SetSlot(cost.Slots[i], resources.Slots.FirstOrDefault((x) => x.type == cost.Slots[i].type));
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
        nameLabel.text = type.displayName;
        levelLabel.text = "LVL " + statInfo.level;
    }
}
