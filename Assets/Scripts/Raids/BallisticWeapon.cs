using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticWeapon : Weapon
{
    protected override void ShootBullet(Transform target, Quaternion spreadRot)
    {
        base.ShootBullet(target, spreadRot);   
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector3 bulletVellosity = BallisticHelper.CalculateVelocity(shootPoint.position, target.position, bullet.Speed);       
        bulletVellosity = spreadRot * bulletVellosity;
        bullet.SetVelocity(bulletVellosity);
        bullet.Damage = Damage;
        Vector3 checkPos = shootPoint.position;
        checkPos.y = target.position.y;
        if (Vector3.Distance(checkPos, target.position) < 2.5f)
        {
            bullet.InstantHit(target);
        }
    }
}
