using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpecialistUpgradeCardUI : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Color unlockedSprite;
    [SerializeField] Color lockedSpite;
    [SerializeField] Button button;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text specialistName;
    [SerializeField] TMP_Text specialistStatLevel;
    [SerializeField] TMP_Text specialistStatValue;
    [SerializeField] Transform resourcesPanel;
    [SerializeField] UIResourceSlot resourceSlotUIPrefab;
    public Animator anim;
     List<UIResourceSlot> resourceSlots = new  List<UIResourceSlot>();
    protected bool unlocked;
    CardController cardController;
    Tween scaleTween;

    private void Awake()
    {
        cardController = CardController.Instance;
    }

    public void AnimUpgrade()
    {
        if (anim) anim.SetTrigger("PressButton");
    }

    public virtual void Setup(Card card, StatsType statType, ResourcesInfo resources, System.Action onClick)
    {
        CardStatsSlot stats = cardController.CardStats(card);
        int statLevel = stats.statsInfo[statType];
        ResourcesInfo levelUpCost = statType.GetLevelCost(statLevel);
        float statValue = card.GetStatValue(statType, statLevel);

        icon.sprite = card.Icon;
        specialistName.text = card.CardName;
        specialistStatLevel.text = "LVL " + (statLevel + 1).ToString();
        specialistStatValue.text = "ATK " + statValue.ToString();


        if (levelUpCost.IsFilled(resources))
        {
            background.color = unlockedSprite;
            unlocked = true;
        }
        else
        {
            background.color = lockedSpite;
            unlocked = false;
        }

        int i = 0;
        for (; i < levelUpCost.Slots.Count; i++)
        {
            if (i < resourceSlots.Count)
                resourceSlots[i].gameObject.SetActive(true);
            else                
                resourceSlots.Add(Instantiate(resourceSlotUIPrefab, resourcesPanel));

            resourceSlots[i].SetSlot(levelUpCost.Slots[i], resources.Slots.FirstOrDefault((x) => x.type == levelUpCost.Slots[i].type));

        }

        for (; i < resourceSlots.Count; i++)
            resourceSlots[i].gameObject.SetActive(false);

      button.interactable = unlocked;
      button.onClick.RemoveAllListeners();
      button.onClick.AddListener(() => OnClicked(onClick));
    } 

    void OnClicked(System.Action onClick)
    {
        transform.localScale = Vector3.one;

        if (scaleTween != null)
        {
            scaleTween.Kill(true);
        }

        scaleTween = transform.DOPunchScale(new Vector2(.1f, .1f), .3f, 7, 1);
        AnimUpgrade();
        onClick?.Invoke();
    }

}
