using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct UnitDamageEventInfo
{
    public float damage;
    public float currentHealth;
    public float maxHealth;
}

public class UnitHealth : MonoBehaviour, IDamagable
{
    public event System.Action<UnitDamageEventInfo> OnTakeDamage;
    public event System.Action OnDead;

    [SerializeField] float health = 100;
    float currentHealth = 0f;


    private void Start()
    {
        currentHealth = health;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        UnitDamageEventInfo info = new UnitDamageEventInfo()
        {
            damage = damage,
            maxHealth = health,
            currentHealth = currentHealth,
        };

        OnTakeDamage?.Invoke(info);
        if (currentHealth <= 0f)
        {
            OnDead?.Invoke();
            Destroy(gameObject);
        }
    }

    public void Damage(DamageInfo info)
    {
        TakeDamage(info.damage);
    }
}
