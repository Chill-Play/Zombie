using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] protected Image background;
    [SerializeField] protected Sprite unlockedSprite;
    [SerializeField] protected Sprite lockedSpite;
    [SerializeField] protected List<UIResourceSlot> resourceSlots;
    [SerializeField] protected Button button;
    [SerializeField] protected Button freeButton;
    protected bool unlocked;


    public virtual void Setup(ResourcesInfo cost, ResourcesInfo resources, bool freeOption, System.Action<bool> OnClick)
    {
        if (cost.IsFilled(resources))
        {
            background.sprite = unlockedSprite;
            unlocked = true;
        }
        else
        {
            background.sprite = lockedSpite;
            unlocked = false;
        }
        for (int i = 0; i < resourceSlots.Count; i++)
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
        button.interactable = unlocked;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClick?.Invoke(false));
        freeButton.gameObject.SetActive(freeOption);
        freeButton.onClick.RemoveAllListeners();
        freeButton.onClick.AddListener(() => OnClick?.Invoke(true));
    }

}
