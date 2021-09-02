using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public event System.Action OnShoot;

    [SerializeField] float bulletDamage = 10f;
    [SerializeField] float fireRate = 0.3f;
    [SerializeField] float spread = 0f;
    [SerializeField] bool uniformSpread = false;
    [SerializeField] int bulletsPerShot = 1;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] ParticleSystem muzzleFx;
    [SerializeField] ParticleSystem shellsFx;
    [SerializeField] float noisePerShoot;

    bool firing;
    float nextFire;
    Transform target;

    public bool Firing => firing;
    public float Damage { get; set; }
    public float FireRate { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Damage += bulletDamage;
        FireRate += fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (firing)
        {
            if (nextFire < Time.time)
            {
                if (target == null)
                {
                    firing = false;
                    return;
                }
                nextFire = Time.time + FireRate;
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    Quaternion spreadRot = Quaternion.identity;
                    if(uniformSpread)
                    {
                        spreadRot = Quaternion.Euler(new Vector3(0f, ((float)i / bulletsPerShot) - 0.5f) * 2f * spread);
                    }
                    else
                    {
                        spreadRot = Quaternion.Euler(new Vector3(0f, Random.Range(-spread, spread)));
                    }
                    Quaternion weaponDirection = Quaternion.LookRotation(new Vector3(shootPoint.forward.x, 0f, shootPoint.forward.z));
                    Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, spreadRot * weaponDirection);
                    bullet.Damage = Damage;
                    if (Vector3.Distance(shootPoint.position, target.position) < 2.5f)
                    {
                        bullet.InstantHit(target);
                    }
                }
                Level.Instance.AddNoiseLevel(noisePerShoot);
                if (muzzleFx != null)
                {
                    muzzleFx.Play();
                }
                if (shellsFx != null)
                {
                    shellsFx.Play();
                }
                OnShoot?.Invoke();
            }
        }
    }


    public void StartFire(Transform target)
    {
        this.target = target;
        firing = true;
    }


    public void StopFire()
    {
        firing = false;
    }
}
