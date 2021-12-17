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

    public float Health => health;
    public float CurrentHealth => currentHealth;

    private void Awake()
    {       
        uiNumbers = FindObjectOfType<UINumbers>();
    }

    public void SetHealth(float value)
    {
        currentHealth = value;
    }

    public void Damage(DamageInfo info)
    {
        if (currentHealth > 0f)
        {
            currentHealth = Mathf.Clamp(currentHealth - info.damage, 0f, health);
            uiNumbers.SpawnNumber(transform.position + Vector3.up * 2f, "-" + info.damage, Vector2.zero, 15f, 10f, 0.4f);
            DamageTakenInfo damageTakenInfo = new DamageTakenInfo();
            damageTakenInfo.damage = info.damage;
            damageTakenInfo.currentHealth = currentHealth;
            damageTakenInfo.maxHealth = health;
            OnDamage?.Invoke(damageTakenInfo);
            if (currentHealth <= 0f)
            {
                OnDead?.Invoke(new EventMessage<Empty>());
            }
        }
       
    }

    public float AddHealth(float value, bool canBeConstructed = false)
    {
        if (currentHealth < health)
        {
            float dH = Mathf.Min(health - currentHealth, value);
            currentHealth += dH;
            if (currentHealth >= health)
            {
                currentHealth = health;
                if (canBeConstructed)
                {
                    OnConstructed?.Invoke();
                }
            }
            DamageTakenInfo damageTakenInfo = new DamageTakenInfo();
            damageTakenInfo.damage = value;
            damageTakenInfo.currentHealth = currentHealth;
            damageTakenInfo.maxHealth = health;
            OnDamage?.Invoke(damageTakenInfo);
            return dH;
        }
        return 0f;
    }
}
