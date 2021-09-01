using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitHealth : MonoBehaviour, IDamagable
{
    public event System.Action<DamageTakenInfo> OnDamage;
    public event System.Action<EventMessage<Empty>> OnDead;

    [SerializeField] float health = 100;

    [SerializeField] ParticleSystem bloodVfx;
    [SerializeField] float bloodVfxScale = 0.2f;


    public float CurrentHealth { get; set; }

    private void Start()
    {
        CurrentHealth += health;
    }


    public void TakeDamage(float damage, Vector3 direction)
    {
        if(CurrentHealth <= 0f)
        {
            return;
        }
        CurrentHealth -= damage;

        if (bloodVfx != null)
        {
            ParticleSystem vfx = Instantiate(bloodVfx, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * bloodVfxScale;
        }
        DamageTakenInfo info = new DamageTakenInfo()
        {
            damage = damage,
            maxHealth = health,
            currentHealth = CurrentHealth,
        };

        OnDamage?.Invoke(info);
        if (CurrentHealth <= 0f)
        {
            var dir = -direction;
            dir.y = 0.0f;
            dir.Normalize();
            transform.forward = dir;
            OnDead?.Invoke(new EventMessage<Empty>(new Empty(), this));
            //Destroy(gameObject);
        }
    }

    public void Damage(DamageInfo info)
    {
        
        TakeDamage(info.damage, info.direction);
    }
}
