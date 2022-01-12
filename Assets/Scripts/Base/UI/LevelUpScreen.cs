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
    private List<GameObject> newResources = new List<GameObject>();
    private List<GameObject> newBuildings = new List<GameObject>();
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Transform resourceSpawnPoint;
    [SerializeField] private GameObject resourcePrefab;
    [SerializeField] private GameObject buildingPrefab;
    private LevelProgressionController levelProgressionController;
    private int resourcesCount;
    HQBuilding hq;
    [SerializeField] int buildingsCount;

    private void Awake()
    {
        levelProgressionController = FindObjectOfType<LevelProgressionController>();
        hq = FindObjectOfType<HQBuilding>();
        hq.OnLevelUp += ShowScreen;
    }

    
    public void CloseScreen()
    {
        //UnlockBuildings();
        
        var seq = DOTween.Sequence();
        seq.Join(continueButton.transform.DOScale(Vector3.zero, .1f));
        seq.AppendInterval(0.1f);
        for (int i = 0; i < buildingsCount; i++)
        {
            seq.Append(newBuildings[i].transform.DOScale(Vector3.zero, .2f).SetEase(Ease.OutCirc));
        }
        for (int i = 0; i < levelProgressionController.CurrentLevelProgression.UnlockResources.Count; i++)
        {
            seq.Append(newResources[i].transform.DOScale(Vector3.zero, .2f).SetEase(Ease.OutCirc));
        }
        seq.Append(newLevelLabel.transform.DOScale(Vector3.zero, .1f));
        seq.AppendInterval(0.1f);
        seq.Append(header.transform.DOScale(Vector3.zero, .1f));
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
        header.transform.localScale = Vector3.zero;
        newLevelLabel.transform.localScale = Vector3.zero;
        continueButton.transform.localScale = Vector3.zero;
        lvlText.text = (hq.Level + 1).ToString();
        int i = 0;
        for (; i < buildingsCount; i++)
        {
            if (i < newBuildings.Count) 
                newResources[i].gameObject.SetActive(true);
            else
            {
                GameObject building = Instantiate(buildingPrefab, Vector3.zero,
                    Quaternion.identity, resourceSpawnPoint);
                newBuildings.Add(building);
            }
            newBuildings[i].transform.localScale = Vector3.zero;
        }
        for (; i < newBuildings.Count; i++)
            newResources[i].gameObject.SetActive(false);
        i = 0;
        for (; i < levelProgressionController.CurrentLevelProgression.UnlockResources.Count; i++)
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
        var seq = DOTween.Sequence();
        seq.Append(header.transform.DOScale(new Vector3(1,1,1), .3f));
        seq.AppendInterval(0.3f);
        seq.Append(newLevelLabel.transform.DOScale(new Vector3(1,1,1), .3f));

        seq.Append(newLevelLabel.transform.DOPunchRotation(new Vector3(0, -50, -30), 1f, 5));
        seq.AppendInterval(0.1f);
        for (int i = 0; i < buildingsCount; i++)
        {
            seq.Join(newBuildings[i].transform.DOScale(new Vector3(1,1,1), .2f).SetEase(Ease.OutCirc));
            seq.AppendInterval(0.1f);
        }
        Debug.Log("resourceCount: " + resourcesCount);
        for (int i = 0; i < levelProgressionController.CurrentLevelProgression.UnlockResources.Count; i++)
        {
            seq.Join(newResources[i].transform.DOScale(new Vector3(1,1,1), .2f).SetEase(Ease.OutCirc));
            seq.AppendInterval(0.1f);
        }
        seq.Append(continueButton.transform.DOScale(new Vector3(1,1,1), .4f));
    }
}
