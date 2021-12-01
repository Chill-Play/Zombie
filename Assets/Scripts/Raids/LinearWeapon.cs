using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearWeapon : Weapon
{
    [SerializeField] bool aimingAtY = false;

    protected override void ShootBullet(Transform target, Quaternion spreadRot)
    {
        base.ShootBullet(target, spreadRot);

        Vector3 targetDirection = target.position - shootPoint.position;
        if (!aimingAtY)
        {
            targetDirection.y = 0.0f;
        }
        targetDirection.Normalize();
        Vector3 shootPointForward = new Vector3(shootPoint.forward.x, 0f, shootPoint.forward.z);
        Quaternion weaponDirection;
        if (Vector3.Angle(targetDirection, shootPointForward) < 10.0f)
        {
            weaponDirection = Quaternion.LookRotation(targetDirection);
        }
        else
        {
            weaponDirection = Quaternion.LookRotation(shootPointForward);
        }
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, spreadRot * weaponDirection);
        bullet.Damage = Damage;
        Vector3 checkPos = shootPoint.position;
        checkPos.y = target.position.y;
        if (Vector3.Distance(checkPos, target.position) < 2.5f)
        {
            bullet.InstantHit(target);
        }
    }

}
