using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnPointAdded += Hq_OnPointAdded;
        pointCountTransform.localScale = Vector3.zero;
        UpdateProgressBar();
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


}
