using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionHealth : MonoBehaviour, IDamagable
{
    public event Action OnConstructed;
    public event Action<EventMessage<Empty>> OnDead;
    public event Action<DamageTakenInfo> OnDamage;

    [SerializeField] float health = 100f;  
    float currentHealth = 0f;
    UINumbers uiNumbers;

    private void Awake()
    {       
        uiNumbers = FindObjectOfType<UINumbers>();
    }

    public void Damage(DamageInfo info)
    {
        currentHealth -= info.damage;
        uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "-" + info.damage, Vector2.zero, 15f, 10f, 0.4f);
        if (currentHealth < 0f)
        {           
            currentHealth = 0f;
            OnDead?.Invoke(new EventMessage<Empty>());
        }
    }

    public float AddHealth(float value)
    {
        float dH = Mathf.Min(health - currentHealth, value);
        currentHealth += dH;
        if (currentHealth >= health)
        {
            currentHealth = health;
            OnConstructed?.Invoke();
        }
        return dH;
    }
}
