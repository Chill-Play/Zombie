using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShooting : MonoBehaviour
{
    public event System.Action OnShoot;
    [SerializeField] UnitTargetDetection unitTargetDetection;
    [SerializeField] Weapon[] weapons;

    public bool AllowShooting { get; set; } = false;

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
        if (unitTargetDetection.Target != null && AllowShooting)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (!weapons[i].Firing)
                {
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
                    weapons[i].StopFire();
                }
            }
        }
    }
}
