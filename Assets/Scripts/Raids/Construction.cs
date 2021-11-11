using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Construction : MonoBehaviour, IDamagable
{

    public event Action<EventMessage<Empty>> OnDead;
    public event Action<DamageTakenInfo> OnDamage;
    public event System.Action OnBuild;

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
                    OnBuild?.Invoke();
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

    public void Damage(DamageInfo info)
    {
        if (constructed)
        {
            health -= info.damage;
            uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "-" + info.damage, Vector2.zero, 15f, 10f, 0.4f);
            if (health < 0f)
            {
                constructed = false;
                currentHealth = 0f;
                OnDead?.Invoke(new EventMessage<Empty>());

                Sequence sequence = DOTween.Sequence();
                sequence.Append(content.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc));
                sequence.AppendCallback(() =>
                {
                    content.gameObject.SetActive(false);
                    ruins.localScale = Vector3.zero;
                    ruins.gameObject.SetActive(true);
                    
                });
                sequence.Append(ruins.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCirc));
            }
        }
    }
}
