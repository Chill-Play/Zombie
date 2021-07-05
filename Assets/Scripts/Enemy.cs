using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    public event System.Action<Enemy> OnDead;

    [SerializeField] float basehealth;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Player player;
    [SerializeField] ParticleSystem bloodVfx;
    [SerializeField] float bloodVfxScale = 0.2f;

    [SerializeField] float attackDamage = 1f;
    [SerializeField] float attackRate = 1.5f;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackTime = 0.5f;
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float healthPerLevel = 20f;
    [SerializeField] float speedPerLevel = 1f;

    float currentHealth;
    float nextAttack;
    bool attackStarted;
    int level;



    public void Damage(DamageInfo info)
    {
        currentHealth -= info.damage;
        ParticleSystem vfx = Instantiate(bloodVfx, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        vfx.transform.localScale = Vector3.one * bloodVfxScale;
        if(currentHealth <= 0f)
        {
            OnDead?.Invoke(this);
            Destroy(gameObject);
        }
    }


    public void SetLevel(int level)
    {
        agent.speed = baseSpeed + (speedPerLevel * level);
        currentHealth = basehealth + (healthPerLevel * level);
        this.level = level;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameplayController.Instance.playerInstance;
        if (player != null && agent.enabled)
        {
            agent.SetDestination(player.transform.position);
        }
        if (nextAttack < Time.time)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < attackRadius)
            {
                nextAttack = Time.time + attackRate;
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    
    public void Stop()
    {
        agent.enabled = false;
        StopAllCoroutines();
    }


    private IEnumerator AttackCoroutine()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(attackTime);
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < attackRadius)
        {
            player.TakeDamage(attackDamage);
        }
        agent.isStopped = false;
    }
}
