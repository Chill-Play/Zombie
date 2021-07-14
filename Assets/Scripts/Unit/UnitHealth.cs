using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitHealth : MonoBehaviour, IDamagable
{
    public event System.Action<DamageTakenInfo> OnDamage;
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

        DamageTakenInfo info = new DamageTakenInfo()
        {
            damage = damage,
            maxHealth = health,
            currentHealth = currentHealth,
        };

        OnDamage?.Invoke(info);
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
