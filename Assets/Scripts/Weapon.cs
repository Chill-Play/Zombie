using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float fireRate;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] ParticleSystem muzzleFx;

    bool firing;
    float nextFire;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firing)
        {
            if (nextFire < Time.time)
            {
                if(target == null)
                {
                    return;
                }
                nextFire = Time.time + fireRate;
                Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                if (Vector3.Distance(shootPoint.position, target.position) < 1f)
                {
                    bullet.InstantHit(target);
                }
                muzzleFx.Play();
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
