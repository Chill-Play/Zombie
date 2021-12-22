using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticWeapon : Weapon
{
    [SerializeField] TargetMarker targetMarkerPrefab;

    protected override void ShootBullet(Transform target)
    {
        base.ShootBullet(target);       
        Debug.Log(shootPoint.position);
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 rInCircle = Random.insideUnitCircle;
        Vector3 spreadPosition = target.position + new Vector3(rInCircle.x * spread, 0f, rInCircle.y * spread);
        if (targetMarkerPrefab != null)
        {
            TargetMarker targetMarker = Instantiate<TargetMarker>(targetMarkerPrefab, spreadPosition, Quaternion.identity);            
            targetMarker.Scale(BallisticHelper.CalculateTime(shootPoint.position, spreadPosition, bullet.Speed));
        }
        Vector3 bulletVellosity = BallisticHelper.CalculateVelocity(shootPoint.position, spreadPosition, bullet.Speed);      
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
