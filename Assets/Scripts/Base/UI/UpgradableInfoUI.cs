using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public class UpgradableInfoUI : MonoBehaviour
{
    [SerializeField] ResourceBar resourceBarPrefab;
    [SerializeField] Transform resourcesLayout;
    [SerializeField] TMP_Text levelText;

    List<ResourceBar> resourceBars = new List<ResourceBar>();
    Dictionary<ResourceType, ResourceBar> resourceBarsDictionary = new Dictionary<ResourceType, ResourceBar>();
    Upgradable upgradable;

    public void Show(Upgradable upgradable)
    {
        this.upgradable = upgradable;
        upgradable.OnUpdateResourcesSpend += UpdateResourcesInfo;
        upgradable.OnLevelUp += Upgradable_OnLevelUp;
        gameObject.SetActive(true);

        levelText.text = "Level " + (upgradable.Level + 1).ToString();

        resourceBarsDictionary.Clear();
        ResourcesInfo resourcesInfo = upgradable.Cost;
        int i = 0;
        for (; i < resourcesInfo.Slots.Count; i++)
        {
            if (i < resourceBars.Count)
                resourceBars[i].gameObject.SetActive(true);
            else
                resourceBars.Add(Instantiate(resourceBarPrefab, resourcesLayout));

            resourceBars[i].Setup(resourcesInfo.Slots[i].type, resourcesInfo.Slots[i].count - upgradable.ResourcesSpent.Count(resourcesInfo.Slots[i].type));
            resourceBarsDictionary.Add(resourcesInfo.Slots[i].type, resourceBars[i]);
        }

        for (; i < resourceBars.Count; i++)
            resourceBars[i].gameObject.SetActive(false);
        transform.DOScale(Vector3.one,.4f).SetEase(Ease.OutElastic, 1.1f, .3f);
    }

    private void Upgradable_OnLevelUp()
    {
        levelText.text = "Level " + (upgradable.Level + 1).ToString();
        foreach (var slot in upgradable.Cost.Slots)
        {
            resourceBarsDictionary[slot.type].Setup(slot.type, slot.count);
        }
    }

    private void UpdateResourcesInfo()
    {
        foreach (var slot in upgradable.ResourcesSpent.Slots)
        {
            foreach (var costSlot in upgradable.Cost.Slots)
            {
                if (costSlot.type == slot.type)
                {
                    var bar = resourceBarsDictionary[slot.type];
                    var value = costSlot.count - slot.count;
                    bar.UpdateValue(value);          
                }
            }
        }
    }

    public void Hide()
    {
        upgradable.OnUpdateResourcesSpend -= UpdateResourcesInfo;
        upgradable.OnLevelUp -= Upgradable_OnLevelUp;
        transform.DOScale(Vector3.zero,.1f).SetEase(Ease.InElastic, 1.1f, .3f).OnComplete(() => { gameObject.SetActive(false); });
    }
}
