using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    public event System.Action OnShoot;

    [SerializeField] protected float bulletDamage = 10f;
    [SerializeField] float fireRate = 0.3f;
    [SerializeField] protected float spread = 0f;
    [SerializeField] bool uniformSpread = false;
    [SerializeField] int bulletsPerShot = 1;
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] ParticleSystem muzzleFx;
    [SerializeField] ParticleSystem shellsFx;
    [SerializeField] float noisePerShoot;

    bool firing;
    float nextFire;
    protected Quaternion spreadRot = Quaternion.identity;
    Transform target;
    IEnumerable<INoiseListener> noiseListeners;

    public bool Firing => firing;
    public float Damage { get; set; }
    public float FireRate { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        noiseListeners = FindObjectsOfType<MonoBehaviour>().OfType<INoiseListener>();
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
                    if (uniformSpread)
                    {
                        spreadRot = Quaternion.Euler(new Vector3(0f, ((float)i / bulletsPerShot) - 0.5f) * 2f * spread);
                    }
                    else
                    {
                        spreadRot = Quaternion.Euler(new Vector3(0f, Random.Range(-spread, spread)));
                    }
                    ShootBullet(target);
                }

                INoiseListener noiseListener = noiseListeners.FirstOrDefault();
                if (noiseListener != null)
                {
                    noiseListener.AddNoiseLevel(noisePerShoot);
                }          

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

    protected virtual void ShootBullet(Transform target)
    {

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
