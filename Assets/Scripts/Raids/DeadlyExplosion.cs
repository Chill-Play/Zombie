using System;
using UnityEngine;
using DG.Tweening;

public class DeadlyExplosion : MonoBehaviour
{
    [SerializeField] private float  damage;
    [SerializeField] LayerMask mask;
    [SerializeField] float radius = 4f;

    private void Start()
    {
        Hit();
        Vector3 scale = transform.localScale;

        transform.localScale = Vector3.zero;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(scale, .25f));
        seq.AppendInterval(1);
        seq.Append(transform.DOScale(Vector3.zero, .5f));
        seq.OnComplete(() => Destroy(gameObject));
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.TryGetComponent(out IDamagable damagable))
    //     {
    //         Hit(damagable);
    //     }
    // }

    public void Hit()
    {
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.color *= new Vector4(1, 1, 1, .5f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}