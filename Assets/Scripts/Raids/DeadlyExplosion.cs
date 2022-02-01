using System;
using UnityEngine;
using DG.Tweening;

public class DeadlyExplosion : MonoBehaviour
{
    [SerializeField] private float  damage;

    private Collider trigger;
    private void Start()
    {
        trigger = GetComponent<Collider>();
        Vector3 scale = transform.localScale;

        transform.localScale = Vector3.zero;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(scale, .25f).OnComplete(() =>
        {
            trigger.enabled = false;
        }));
        seq.AppendInterval(1);
        seq.Append(transform.DOScale(Vector3.zero, .5f));
        seq.OnComplete(() => Destroy(gameObject));
    }

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