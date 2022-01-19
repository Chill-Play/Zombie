using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public class RewardScreen : MonoBehaviour
{
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject VFX;
    [SerializeField] private Image chestSprite;
    [SerializeField] private Sprite chestIcon;
    [SerializeField] private Sprite openedChestIcon;
    [SerializeField] private GameObject rewardText;
    [SerializeField] private GameObject claimButton;
    [SerializeField] private ResourceBar resourcePrefab;
    [SerializeField] private Transform resourcesSpawnPoint;
    private List<ResourceBar> resources = new List<ResourceBar>();
    private LevelProgressionSettings levelSettings;
    private int chestCount;
    private int resourcesCount;
    private ResourcesInfo resInfo;
    InputPanel inputPanel;

    private void Awake()
    {
        inputPanel = FindObjectOfType<InputPanel>();
    }

    public void OpenChest(int i)
    {
        inputPanel.DisableInput();
        VFX.transform.localScale = Vector3.zero;
        chest.transform.localScale = Vector3.zero;
        rewardText.transform.localScale = Vector3.zero;
        claimButton.transform.localScale = Vector3.zero;
        levelSettings = FindObjectOfType<LevelProgressionController>().CurrentLevelProgression;
        chestSprite.sprite = levelSettings.Chests[i].chestInfo.Icon;
        chestSprite.sprite = levelSettings.Chests[i].chestInfo.OpenedIcon;
        resInfo = levelSettings.Chests[i].resourcesInfo;
        resourcesCount = resInfo.Slots.Count;
        
        int j = 0;
        for (; j < resourcesCount; j++)
        {
            if (j < resources.Count)
                resources[j].gameObject.SetActive(true);
            else
            {
                ResourceBar newResourceIcon = Instantiate(resourcePrefab, resourcesSpawnPoint);
                resources.Add(newResourceIcon);
            }
            resources[j].Setup(resInfo.Slots[j].type, 0);
            resources[j].transform.localScale = Vector3.zero;
            resources[j].transform.GetChild(0).GetComponent<Image>().sprite = resInfo.Slots[j].type.icon;
            resources[j].gameObject.SetActive(true);
        }

        for (; j < resources.Count; j++)
            resources[j].gameObject.SetActive(false);
        OpenChestAnimation();
    }
    
    void ResourceCallback(int i)
    {
        ResourceBar resourceBar = resources[i].GetComponent<ResourceBar>();
        resourceBar.UpdateValue(resInfo.Slots[i].count);
    }

    void CompleteCloseScreen()
    {
        inputPanel.EnableInput();
        gameObject.SetActive(false);
    }
    
    public void CloseScreen()
    {
        var seq = DOTween.Sequence();
        seq.Append(claimButton.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InBack));
        seq.AppendInterval(0.1f);
        for (int i = 0; i < resourcesCount; i++)
        {
            seq.Join(resources[i].transform.DOScale(Vector3.zero,  .2f + .2f * i).SetEase(Ease.InBack));
        }
        seq.Append(VFX.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InBack));
        seq.Join(chest.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InBack));
        seq.Join(rewardText.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InBack));
        seq.AppendInterval(0.2f);
        seq.OnComplete(CompleteCloseScreen);
    }
    
    void OpenChestAnimation()
    {
        chestSprite.sprite = chestIcon;
        var seq = DOTween.Sequence();
        seq.Append(rewardText.transform.DOScale(new Vector3(1,1,1), .3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.3f);
        seq.Join(chest.transform.DOScale(new Vector3(1,1,1), .3f).SetEase(Ease.OutBack));
        seq.Join(VFX.transform.DOScale(new Vector3(1,1,1), .4f));
        seq.Append(chest.transform.DOPunchRotation(new Vector3(0, 0, 30), 1f, 5).SetEase(Ease.OutBack)).AppendCallback(() =>
        {
            chestSprite.sprite = openedChestIcon;
        });
        seq.AppendInterval(0.1f);
        for (int i = 0; i < resourcesCount; i++)
        {
            int tmp = i;
            
            seq.Join(resources[i].transform.DOScale(new Vector3(1,1,1), .3f + .3f * i).OnPlay(()=>ResourceCallback(tmp)).SetEase(Ease.OutBack));
        }
        seq.Append(claimButton.transform.DOScale(new Vector3(1,1,1), .4f).SetEase(Ease.OutBack));
    }
}