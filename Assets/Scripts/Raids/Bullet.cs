using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Linear,
        Ballistic
    }

    [SerializeField] float speed;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] float timeToDestroy = 10f;
    [SerializeField] BulletType bulletType;
    Vector3 lastPosition;

    public float Damage { get; set; }
    public float Speed => speed;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (bulletType == BulletType.Linear)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (Physics.Linecast(lastPosition, transform.position, out RaycastHit hit, collisionMask) || transform.position.y < 0f)
        {
            Hit(hit.transform);
        }
        lastPosition = transform.position;
    }

    protected virtual void Hit(Transform target)
    {
        if (target.TryGetComponent(out IDamagable damagable))
        {
            DamageInfo damageInfo = new DamageInfo()
            {
                direction = transform.forward,
                damage = Damage,
            };
            damagable.Damage(damageInfo);
        }
        Destroy(gameObject);
    }

    public void InstantHit(Transform target)
    {
        Hit(target);
    }

    public void SetVelocity(Vector3 velocity)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = velocity; 
    }
}
