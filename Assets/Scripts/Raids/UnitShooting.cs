using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShooting : MonoBehaviour
{
    public event System.Action OnShoot;
    [SerializeField] float radius;
    [SerializeField] Weapon[] weapons;
    Transform target;

    public bool AllowShooting { get; set; } = true;
    public Transform Target => target;

    Collider[] attackColliders = new Collider[30];
    // Start is called before the first frame update
    void OnEnable()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].OnShoot += UnitShooting_OnShoot;
        }
    }


    void OnDisable()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].OnShoot -= UnitShooting_OnShoot;
        }
    }


    private void UnitShooting_OnShoot()
    {
        OnShoot?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        int collidersFound = Physics.OverlapSphereNonAlloc(transform.position, radius, attackColliders, GameplayUtils.ATTACK_MASK);
        target = GameplayUtils.GetAttackTarget(0, collidersFound, transform, attackColliders);
        if (target != null && AllowShooting)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (!weapons[i].Firing)
                {
                    weapons[i].StartFire(target);
                }
            }
        }
        else
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].Firing)
                {
                    weapons[i].StopFire();
                }
            }
        }
    }
}
