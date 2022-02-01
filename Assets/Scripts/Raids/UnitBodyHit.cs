using System;
using UnityEngine;

public class UnitBodyHit : MonoBehaviour
{
    [SerializeField] private float damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            Hit(damagable);
        }
    }

    public void Hit(IDamagable damagable)
    {
        DamageInfo damageInfo = new DamageInfo()
        {
            direction = transform.forward,
            damage = damage,
        };
        damagable.Damage(damageInfo);
    }
}