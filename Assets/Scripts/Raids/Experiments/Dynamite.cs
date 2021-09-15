using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dynamite : MonoBehaviour, IPlayerTool
{
    [SerializeField] float detonateTime = 3f;
    [SerializeField] float radius = 4f;
    [SerializeField] float damage = 200f;
    [SerializeField] LayerMask mask;

    public void Detonate()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        StartCoroutine(DetonateCoroutine());
    }

    public void UseTool()
    {
        Detonate();
    }

    IEnumerator DetonateCoroutine()
    {
        float timer = 0f;
        Vector3 scale = transform.localScale;
        transform.DOLocalRotate(new Vector3(0f, 360f, 0f), detonateTime, RotateMode.LocalAxisAdd);
        while (timer <= detonateTime)
        {
            transform.DOPunchScale(scale * 0.3f, 0.3f);
            yield return new WaitForSeconds(1f);
            timer += 1f;          
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, mask);
        foreach (var hitCollider in hitColliders)
        {            
            IDamagable damagable = hitCollider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = damage;
                Vector3 dir = hitCollider.transform.position - transform.position;
                dir.Normalize();
                damageInfo.direction = dir;
                damagable.Damage(damageInfo);
            }
        }
        Destroy(gameObject);               
    }
}
