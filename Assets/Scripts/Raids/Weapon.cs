using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                    Quaternion spreadRot = Quaternion.identity;
                    if(uniformSpread)
                    {
                        spreadRot = Quaternion.Euler(new Vector3(0f, ((float)i / bulletsPerShot) - 0.5f) * 2f * spread);
                    }
                    else
                    {
                        spreadRot = Quaternion.Euler(new Vector3(0f, Random.Range(-spread, spread)));
                    }
                    Vector3 targetDirection = target.position - shootPoint.position;
                    targetDirection.y = 0.0f;
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
