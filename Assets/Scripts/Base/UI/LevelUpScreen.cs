using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour
{
    [SerializeField] private GameObject header;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private GameObject newLevelLabel;
    [SerializeField] private Image background;
    private List<GameObject> newResources = new List<GameObject>();
    private List<GameObject> newBuildings = new List<GameObject>();
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Transform resourceSpawnPoint;
    [SerializeField] private GameObject resourcePrefab;
    [SerializeField] private GameObject buildingPrefab;
    private LevelProgressionController levelProgressionController;
    HQBuilding hq;
    InputPanel inputPanel;
    private float a;
    private int resourcesCount => levelProgressionController.CurrentLevelProgression.UnlockResources.Count;
    private int buildingCount => levelProgressionController.UnlockableBuildings.Count;

    
    private void Awake()
    {
        inputPanel = FindObjectOfType<InputPanel>();
        levelProgressionController = FindObjectOfType<LevelProgressionController>();
        hq = FindObjectOfType<HQBuilding>();
        a = background.color.a;
        hq.OnLevelUp += ShowScreen;
    }

    
    public void CloseScreen()
    {
        inputPanel.EnableInput();
        var seq = DOTween.Sequence();
        seq.Join(continueButton.transform.DOScale(Vector3.zero, .1f).SetEase(Ease.InBack));
        seq.AppendInterval(0.1f);
        for (int i = 0; i < buildingCount; i++)
        {
            int tmp = i;
            seq.AppendInterval(i * .1f);
            seq.AppendCallback(() =>
            {
                newBuildings[tmp].transform.DOScale(Vector3.zero, .2f + .2f * i).SetEase(Ease.InBack);
            });
        }
        for (int i = 0; i < resourcesCount; i++)
        {
            int tmp = i;
            seq.AppendInterval((i+buildingCount) * .1f);
            seq.AppendCallback(() =>
            {
                newResources[tmp].transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InBack);
            });
        }
        seq.Append(newLevelLabel.transform.DOScale(Vector3.zero, .1f).SetEase(Ease.InBack));
        seq.AppendInterval(0.1f);
        seq.Append(header.transform.DOScale(Vector3.zero, .1f).SetEase(Ease.InBack));
        seq.Append(DOTween.ToAlpha(() => background.color, x => background.color = x, 0, .25f));
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            levelProgressionController.UnlockBuildings();
            levelProgressionController.UnlockResourceType();
        });
    }

    public void ShowScreen()
    {        
        gameObject.SetActive(true);
        inputPanel.DisableInput();
        header.transform.localScale = Vector3.zero;
        newLevelLabel.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;
        lvlText.text = (hq.Level + 1).ToString();
        int i = 0;
        for (; i < buildingCount; i++)
        {
            if (i < newBuildings.Count) 
                newBuildings[i].gameObject.SetActive(true);
            else
            {
                GameObject building = Instantiate(buildingPrefab, Vector3.zero,
                    Quaternion.identity, resourceSpawnPoint);
                newBuildings.Add(building);
            }
            newBuildings[i].transform.localScale = Vector3.zero;
        }
        for (; i < newBuildings.Count; i++)
            newBuildings[i].gameObject.SetActive(false);
        i = 0;
        for (; i < resourcesCount; i++)
        {
            if (i < newResources.Count) 
                newResources[i].gameObject.SetActive(true);
            else
            {
                GameObject res = Instantiate(resourcePrefab, Vector3.zero,
                    Quaternion.identity, resourceSpawnPoint);
                newResources.Add(res);
            }
            newResources[i].transform.localScale = Vector3.zero;
            Sprite resourceSprite = levelProgressionController.CurrentLevelProgression.UnlockResources[i].icon;
            newResources[i].transform.GetChild(0).GetComponent<Image>().sprite = resourceSprite;
        }
        for (; i < newResources.Count; i++)
            newResources[i].gameObject.SetActive(false);
        PlayScreenAnimation();
    }
    
    void PlayScreenAnimation()
    {
        background.color = new Color(0,0,0,0);
        var seq = DOTween.Sequence();
        seq.Append(DOTween.ToAlpha(() => background.color, x => background.color = x, a, .25f));
        seq.AppendInterval(0.5f);
        seq.Append(header.transform.DOScale(new Vector3(1,1,1), .3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.3f);
        seq.Append(newLevelLabel.transform.DOScale(new Vector3(1,1,1), .3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.5f);
        for (int i = 0; i < buildingCount; i++)
        {
            int tmp = i;
            seq.AppendInterval(i * .1f);
            seq.AppendCallback(() =>
            {
                newBuildings[tmp].transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutBack);
            });
            //seq.Join(newBuildings[i].transform.DOScale(new Vector3(1,1,1), .4f + .4f * i).SetEase(Ease.OutBack));
        }
        for (int i = 0; i < resourcesCount; i++)
        {
            int tmp = i;
            seq.AppendInterval((i+buildingCount) * .1f);
            seq.AppendCallback(() =>
            {
                newResources[tmp].transform.DOScale(new Vector3(1, 1, 1), .4f).SetEase(Ease.OutBack);
            });
        }

        seq.AppendInterval(.75f);
        seq.Append(continueButton.transform.DOScale(new Vector3(1,1,1), .4f).SetEase(Ease.OutBack));
    }
}
