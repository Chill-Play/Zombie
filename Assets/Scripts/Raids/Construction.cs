using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Construction : MonoBehaviour , IZombiesLevelPhases
{   
    public event System.Action OnBuild;
    public event System.Action OnBreak;

    [SerializeField] Transform ruins;
    [SerializeField] Transform content;
    [SerializeField] bool constructed = false;

    UINumbers uiNumbers;
   ConstructionHealth constructionHealth;

    public bool Constructed => constructed;

    public bool LockConstruction { get; set; } = false; 

    private void Awake()
    {
        content.gameObject.SetActive(constructed);
        ruins.gameObject.SetActive(!constructed);       
        uiNumbers = FindObjectOfType<UINumbers>();
        constructionHealth = content.GetComponent<ConstructionHealth>();      
        constructionHealth.OnConstructed += ConstructionHealth_OnConstructed;
        constructionHealth.OnDead += ConstructionHealth_OnDead;
    }

    private void ConstructionHealth_OnDead(EventMessage<Empty> obj)
    {     
        constructed = false;
        LockConstruction = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(content.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc));
        sequence.AppendCallback(() =>
        {
            content.gameObject.SetActive(false);
            ruins.localScale = Vector3.zero;
            ruins.gameObject.SetActive(true);
            OnBreak?.Invoke();
        });
        sequence.Append(ruins.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCirc));
    }

    private void ConstructionHealth_OnConstructed()
    {       
        constructed = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(ruins.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc));
        sequence.AppendCallback(() =>
        {
            ruins.gameObject.SetActive(false);
            content.localScale = Vector3.zero;
            content.gameObject.SetActive(true);
            OnBuild?.Invoke();
        });
        sequence.Append(content.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCirc));      
    }

    public float Construct(float value)
    {
        if (!constructed && !LockConstruction)
        {
            float dH = constructionHealth.AddHealth(value);
            uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "+" + dH, Vector2.zero, 15f, 10f, 0.4f);
            return dH;
        }
        return 0f;
    }

    public float Repair(float value)
    {
        if (constructed && !LockConstruction)
        {
            float dH = constructionHealth.AddHealth(value);            
            uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "+" + dH, Vector2.zero, 15f, 10f, 0.4f);
            return dH;
        }
        return 0f;
    }

    public void OnLevelStarted()
    {
        
    }

    public void OnLevelEnded()
    {
        
    }

    public void OnLevelFailed()
    {
        
    }

    public void OnHordeDefeated()
    {
        LockConstruction = true;
    }
}
