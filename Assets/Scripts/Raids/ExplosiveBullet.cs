using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [SerializeField] Explosion explosionPrefab;  

    protected override void Hit(Transform target)
    {
        //base.Hit(target);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
