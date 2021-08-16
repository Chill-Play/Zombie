using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeBar : MonoBehaviour
{
    [SerializeField] TMP_Text statNameText;
    [SerializeField] TMP_Text statCountText;

    public void SetupBar(StatsType type, int count)
    {
        statNameText.text = type.displayName;
        statCountText.text = count.ToString();
    }

    public void UpdateValue(int count)
    {      
        statCountText.text = count.ToString();
    }





}
