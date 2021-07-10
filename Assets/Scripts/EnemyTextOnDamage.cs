using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTextOnDamage : MonoBehaviour
{
    [SerializeField] float upOffset = 2f;
    [SerializeField] float randomAngle = 15f;
    [SerializeField] float randomOffset = 10f;
    [SerializeField] Enemy enemy;
    // Start is called before the first frame update
    void OnEnable()
    {
        enemy.OnDamage += Enemy_OnDamage;
    }


    void OnDisable()
    {
        enemy.OnDamage -= Enemy_OnDamage;
    }


    private void Enemy_OnDamage(DamageTakenInfo obj)
    {
        FindObjectOfType<UINumbers>().SpawnNumber(transform.position + Vector3.up * upOffset, "-" + obj.damage, Vector2.zero, randomAngle, randomOffset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
