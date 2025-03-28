using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text text;

    int targetValue;
    float currentValue;
    ResourceType resourceType;

    public void Setup(ResourceType type, int count)
    {
        resourceType = type;
        icon.sprite = type.icon;
        text.text = count.ToString();
        targetValue = count;
        currentValue = count;
    }


    private void Update()
    {
        float newValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 10f);
        if(newValue != currentValue)
        {
            text.text = Mathf.RoundToInt(newValue).ToString();
            currentValue = newValue;
        }
    }


    public void UpdateValue(int newValue)
    {
        if(targetValue == newValue)
        {
            return;
        }
        text.transform.DOKill(true);
        text.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 3);
        targetValue = newValue;
    }
}
