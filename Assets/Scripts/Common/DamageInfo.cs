using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public Vector3 direction;
    public float damage;

    public DamageInfo(Vector3 direction, float damage)
    {
        this.direction = direction;
        this.damage = damage;
    }
}
