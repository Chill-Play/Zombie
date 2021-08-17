using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarWhithMaxCount : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text text;

    int maxValue; 
    int targetValue;
    float currentValue;
    ResourceType resourceType;

    public void Setup(ResourceType type, int count, int maxValue)
    {
        resourceType = type;
        icon.sprite = type.icon;     
        targetValue = maxValue - count;
        currentValue = maxValue - count;
        text.text = currentValue.ToString() + "/" + maxValue.ToString();
        this.maxValue = maxValue;
    }


    private void Update()
    {
        float newValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 10f);
        if(newValue != currentValue)
        {
            text.text = Mathf.RoundToInt(newValue).ToString() + "/" + maxValue.ToString();
            currentValue = newValue;
        }
    }


    public void UpdateValue(int newValue)
    {
        newValue = maxValue - newValue;
        if (targetValue == newValue)
        {
            return;
        }
        text.transform.DOKill(true);
        text.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 3);
        targetValue = newValue;
    }
}
