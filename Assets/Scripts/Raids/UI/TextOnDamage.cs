using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnDamage : MonoBehaviour
{
    [SerializeField] float upOffset = 2f;
    [SerializeField] float randomAngle = 15f;
    [SerializeField] float randomOffset = 10f;
    IDamagable damagable;
    UINumbers uiNumbers;

    private void Awake()
    {
        uiNumbers = UINumbers.Instance;
    }

    void OnEnable()
    {
        damagable = GetComponent<IDamagable>();       
        damagable.OnDamage += Enemy_OnDamage;
    }


    void OnDisable()
    {
        damagable.OnDamage -= Enemy_OnDamage;
    }


    private void Enemy_OnDamage(DamageTakenInfo obj)
    {
        uiNumbers.SpawnNumber(transform.position + Vector3.up * upOffset, "-" + obj.damage, Vector2.zero, randomAngle, randomOffset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
