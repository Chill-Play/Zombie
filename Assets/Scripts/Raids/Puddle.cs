using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime = 3;
    
    private float tmpSpeed;
    
    private void Start()
    {
        Vector3 scale = transform.localScale;
        
        transform.localScale = Vector3.zero;
        tmpSpeed = speed;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOScale(scale, .75f));
        seq.AppendInterval(lifeTime);
        seq.Append(transform.DOScale(Vector3.zero, .5f));
        seq.OnComplete(()=>Destroy(gameObject));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            tmpSpeed -= Time.deltaTime;
            if (tmpSpeed > 0)
                return;
            Hit(damagable);
            tmpSpeed = speed;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.color *= new Vector4(1, 1, 1, .5f);
        Gizmos.DrawSphere(transform.position , GetComponent<SphereCollider>().radius);
    }
}