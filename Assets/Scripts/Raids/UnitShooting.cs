using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShooting : UnitFighting
{
    public event System.Action OnShoot;
    [SerializeField] UnitTargetDetection unitTargetDetection;  
    [SerializeField] Weapon[] weapons;
    [SerializeField] bool allowShooting = false;


    public bool AllowShooting { get; set; } = false;

    // Start is called before the first frame update
    private void Awake()
    {
        AllowShooting = allowShooting;
    }

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
            if (weapons[i].Firing)
            {
                Attacking = false;
                weapons[i].StopFire();
            }
            weapons[i].OnShoot -= UnitShooting_OnShoot;
        }     
    }


    private void UnitShooting_OnShoot()
    {
        OnShoot?.Invoke();
    }


    public void AddDamage(float damage)
    {
        foreach(var weapon in weapons)
        {
            weapon.Damage += damage;
        }
    }


    public void AddAttackRate(float rate)
    {
        foreach (var weapon in weapons)
        {
            weapon.FireRate += rate;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (unitTargetDetection.Target != null && AllowShooting)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (!weapons[i].Firing)
                {
                    Attacking = true;
                    weapons[i].StartFire(unitTargetDetection.Target);                   
                }
            }
        }
        else
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].Firing)
                {
                    Attacking = false;
                    weapons[i].StopFire();                  
                }
            }
        }
    }
}
