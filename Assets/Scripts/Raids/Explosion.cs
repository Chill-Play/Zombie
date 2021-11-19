using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float damage = 10f;
    [SerializeField] LayerMask collisionMask;

    Collider[] targets = new Collider[40];

    void Start()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, targets, collisionMask);

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (targets[i].TryGetComponent(out IDamagable damagable))
                {
                    DamageInfo damageInfo = new DamageInfo()
                    {
                        direction = transform.forward,
                        damage = damage,
                    };
                    damagable.Damage(damageInfo);
                }

            }
        }
    }   
}
