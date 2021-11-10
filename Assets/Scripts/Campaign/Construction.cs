using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Construction : MonoBehaviour
{
    [SerializeField] Transform ruins;
    [SerializeField] Transform content;
    [SerializeField] float health = 100f;
    [SerializeField] bool constructed = false;

    float currentHealth = 0f;
    UINumbers uiNumbers;

    public bool Constructed => constructed;


    private void Awake()
    {
        content.gameObject.SetActive(constructed);
        ruins.gameObject.SetActive(!constructed);
        uiNumbers = FindObjectOfType<UINumbers>();
    }

    public float Construct(float value)
    {
        if (!constructed)
        {
            float dH = AddHealth(value);
            uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "+" + dH, Vector2.zero, 15f, 10f, 0.4f);
            if (currentHealth >= health)
            {
                constructed = true;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(ruins.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc));
                sequence.AppendCallback(() => {
                    ruins.gameObject.SetActive(false);
                    content.localScale = Vector3.zero;
                    content.gameObject.SetActive(true);
                });
                sequence.Append(content.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCirc));                
            }
            return dH;
        }
        return 0f;
    }

    public float Repair(float value)
    {
        if (constructed)
        {
            return AddHealth(value);
        }
        return 0f;
    }

    float AddHealth(float value)
    {
        float dH =  Mathf.Min(health - currentHealth, value);
        currentHealth += dH;
        return dH;
    }

}
