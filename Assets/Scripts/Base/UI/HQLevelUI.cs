using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class HQLevelUI : MonoBehaviour
{
    [SerializeField] Image progressionImage;
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField] TMP_Text levelLebel;
    [SerializeField] TMP_Text pointCountText;
    [SerializeField] Transform pointCountTransform;
    [SerializeField] float disappearTime = 4f;
    [SerializeField] Transform OMG;
    HQBuilding hq;
    float currentDisappearTime = 0f;
    int pointCombo;
    Tween pointCountTween;

    [SerializeField] private Transform giftsSpawnPoint;
    [SerializeField] GameObject rewardPrefab;
    [SerializeField] private Sprite openRewardSprite;
    [SerializeField] private Sprite rewardSprite;
    int rewardSegment;
    private List<GameObject> chests = new List<GameObject>();

    [SerializeField] private RewardScreen rewardScreen;

    private void Start()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnPointAdded += Hq_OnPointAdded;
        hq.OnLevelUp += Hq_OnLevelUp;
        hq.OnRewardOpened += Hq_OnRewardOpened;
        pointCountTransform.localScale = Vector3.zero;
        UpdateProgressBar();
        RewardPlacement();
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
        currentLevelText.text = (hq.Level + 1).ToString();
        nextLevelText.text = (hq.Level + 2).ToString();
        levelLebel.text = "HQ LEVEL " + (hq.Level + 1).ToString();
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

    private void RewardPlacement()
    {
        Transform levelBar = transform.GetChild(0);        
        float width = OMG.GetComponent<RectTransform>().sizeDelta.x;
        float segment = width / (hq.RewardCount + 1);
        Debug.Log(width);
        Vector3 startPos = giftsSpawnPoint.InverseTransformPoint(OMG.transform.position - new Vector3(width / 2f, 0, 0));
        int nextChest = hq.NextChest;

        int i = 0;
        for (; i < hq.RewardCount; i++)
        {
            if (i < chests.Count) 
                chests[i].gameObject.SetActive(true);
            else
            {
                GameObject newChest = Instantiate(rewardPrefab, Vector3.zero,
                    Quaternion.identity, giftsSpawnPoint);
                chests.Add(newChest);
            }
            Debug.Log(startPos);
            // Debug.Log(startPos + new Vector3(segment * (i + 1), 20, 0));
            Debug.Log(startPos + new Vector3(segment * (i + 1), 20, 0));
            chests[i].transform.localPosition = startPos + new Vector3(segment * (i + 1), 20, 0);
            chests[i].transform.GetChild(0).GetComponent<Image>().sprite = (i < nextChest) ? openRewardSprite : rewardSprite;
        }
        for (; i < chests.Count; i++)
            chests[i].gameObject.SetActive(false);
    }

    private void Hq_OnLevelUp()
    {
        RewardPlacement();
    }


    void Complete(int index)
    {
        
        rewardScreen.gameObject.SetActive(true);
        rewardScreen.OpenChest(index);
    }
    
    private void Hq_OnRewardOpened(int index)
    {
        giftsSpawnPoint.GetChild(index).GetChild(0).GetComponent<Image>().sprite = openRewardSprite;
        //play open chest animation
        var seq = DOTween.Sequence();
        seq.Append(chests[index].transform.DOShakeScale(.5f, 1, 7));
        seq.OnComplete(()=>Complete(index));
    }
    
    //Mathematical division of the level bar into segments
    // private void RewardPlacement()
    // {
    //     Transform levelBar = transform.GetChild(0); 
    //     float width = levelBar.GetComponent<RectTransform>().sizeDelta.x;
    //     float segmentWidth = width / hq.Cost;
    //     rewardSegment = hq.Cost / (hq.RewardCount + 1);
    //     Vector3 startPos = levelBar.position - new Vector3(width / 2, 0, 0);
    //     int nextChest = hq.NextChest;
    //
    //     for (int i = 0; i < hq.RewardCount; i++)
    //     {
    //         Instantiate(rewardPrefab, startPos + new Vector3(segmentWidth * rewardSegment * (i + 1), 20, 0),
    //             Quaternion.identity, giftsSpawnPoint);
    //         if (i < nextChest)
    //         {
    //             giftsSpawnPoint.GetChild(i).GetChild(0).GetComponent<Image>().sprite = openRewardSprite;
    //         }
    //     }
    // }
}
