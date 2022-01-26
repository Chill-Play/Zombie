using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HQBanner : MonoBehaviour
{ 
    [SerializeField] TMP_Text levelText;

    HQBuilding hqBuilding;
    Vector3 scale;

    private void OnEnable()
    {
        hqBuilding = FindObjectOfType<HQBuilding>();
        hqBuilding.OnLevelUp += HqBuilding_OnLevelUp; 
        scale = transform.localScale;  
    }

    private void Start()
    {
        levelText.text = "lvl " + (hqBuilding.Level + 1).ToString();
    }

    private void HqBuilding_OnLevelUp(int level)
    {
        levelText.text = "lvl " + (hqBuilding.Level + 1).ToString();
        transform.DOKill(true);
        transform.DOPunchScale(scale * 0.1f, 0.3f);
    }  

    private void OnDisable()
    {
        if (hqBuilding != null)
        {
            hqBuilding.OnLevelUp -= HqBuilding_OnLevelUp;
        }
    }
}
