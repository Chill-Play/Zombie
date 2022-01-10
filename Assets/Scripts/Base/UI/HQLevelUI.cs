using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HQLevelUI : MonoBehaviour
{
    [SerializeField] Image progressionImage;
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField] TMP_Text levelLebel;
    [SerializeField] TMP_Text pointCountText;
    [SerializeField] Transform pointCountTransform;
    [SerializeField] float disappearTime = 4f;
    HQBuilding hq;
    float currentDisappearTime = 0f;
    int pointCombo;
    Tween pointCountTween;

    [SerializeField] private Transform giftsSpawnPoint;
    [SerializeField] GameObject rewardPrefab;
    [SerializeField] private Sprite openRewardSprite;
    int rewardSegment;

    private void Start()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnPointAdded += Hq_OnPointAdded;
        hq.OnLevelUp += Hq_OnLevelUp;
        hq.OnRewardOpened += Hq_OnRewardOpened;
        pointCountTransform.localScale = Vector3.zero;
        UpdateProgressBar();
        RewardPlacement();
        //UpdateRewards();
    }

    private void Hq_OnPointAdded(int value)
    {      
        currentDisappearTime = disappearTime;
        if (pointCountTween != null)
        {
            pointCountTween.Kill(true);
        }
        if (pointCombo == 0)
        {
            pointCountTransform.localScale = Vector3.one;
            StartCoroutine(DisappearCoroutine());
        }
        pointCombo += value;
        pointCountText.text = "+" + pointCombo.ToString();  
        pointCountTween = pointCountTransform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 1, 1);
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        progressionImage.fillAmount = ((float)hq.CurrentCount / (float)hq.Cost);
        currentLevelText.text = hq.Level.ToString();
        nextLevelText.text = (hq.Level + 1).ToString();
        levelLebel.text = "HQ LEVEL " + hq.Level.ToString();
    }

    IEnumerator DisappearCoroutine()
    {       
        pointCountTransform.gameObject.SetActive(true);
        while (currentDisappearTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentDisappearTime--;
        }
        pointCombo = 0;
        pointCountTransform.gameObject.SetActive(false);
        pointCountTween = pointCountTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutCirc);
    }

    // private void RewardPlacement()
    // {
    //     Transform levelBar = transform.GetChild(0);
    //     int width = (int) levelBar.GetComponent<RectTransform>().sizeDelta.x;
    //     int segment = width / (hq.RewardCount + 1);
    //     Vector3 startPos = levelBar.position - new Vector3(width / 2, 0, 0);
    //     
    //     for (int i = 0; i < hq.RewardCount; i++)
    //         Instantiate(rewardPrefab, startPos + new Vector3(segment * (i + 1), 20, 0), Quaternion.identity, levelBar);
    // }

    private void Hq_OnLevelUp()
    {
        RewardRemove();
        RewardPlacement();
    }
    private void UpdateRewards()
    {
        for (int i = 1; i <= hq.RewardCount; i++)
        {
            if (hq.CurrentCount >= rewardSegment * i)
            {
                while (i > 0)
                    giftsSpawnPoint.GetChild(--i).GetChild(0).GetComponent<Image>().sprite = openRewardSprite;
                break;
            }
        }
    }

    private void Hq_OnRewardOpened(int index)
    {
        giftsSpawnPoint.GetChild(index).GetChild(0).GetComponent<Image>().sprite = openRewardSprite;
    }
    
    private void RewardPlacement()
    {
        Transform levelBar = transform.GetChild(0); 
        float width = levelBar.GetComponent<RectTransform>().sizeDelta.x;
        float segmentWidth = width / hq.Cost;
        rewardSegment = hq.Cost / (hq.RewardCount + 1);
        Vector3 startPos = levelBar.position - new Vector3(width / 2, 0, 0);
        int nextChest = hq.NextChest;

        for (int i = 0; i < hq.RewardCount; i++)
        {
            Instantiate(rewardPrefab, startPos + new Vector3(segmentWidth * rewardSegment * (i + 1), 20, 0),
                Quaternion.identity, giftsSpawnPoint);
            if (i < nextChest)
            {
                giftsSpawnPoint.GetChild(i).GetChild(0).GetComponent<Image>().sprite = openRewardSprite;
            }
        }
    }

    private void RewardRemove()
    {
        foreach (Transform child in giftsSpawnPoint.transform) {
            Destroy(child.gameObject);
        }
    }
}
