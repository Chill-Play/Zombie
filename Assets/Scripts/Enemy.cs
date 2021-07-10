using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct DamageTakenInfo
{
    public float damage;
    public float currentHealth;
    public float maxHealth;
}



public class Enemy : MonoBehaviour, IDamagable
{
    public event System.Action<Enemy> OnDead;
    public event System.Action<DamageTakenInfo> OnDamage;

    [SerializeField] float basehealth;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] ParticleSystem bloodVfx;
    [SerializeField] float bloodVfxScale = 0.2f;
    [SerializeField] bool healthBar;

    [SerializeField] float attackDamage = 1f;
    [SerializeField] float attackRate = 1.5f;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackTime = 0.5f;
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float healthPerLevel = 20f;
    [SerializeField] float speedPerLevel = 1f;
    [SerializeField] LayerMask attackMask;


    Squad squad;
    float maxHealth;
    float currentHealth;
    float nextAttack;
    bool attackStarted;
    int level = -1;
    Collider[] attackColliders = new Collider[5];



    public void Damage(DamageInfo info)
    {
        if(currentHealth <= 0f)
        {
            return;
        }
        currentHealth -= info.damage;
        ParticleSystem vfx = Instantiate(bloodVfx, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        vfx.transform.localScale = Vector3.one * bloodVfxScale;
        if(currentHealth <= 0f)
        {
            OnDead?.Invoke(this);
            if (healthBar)
            {
                FindObjectOfType<UIEnemyHealthBars>().RemoveHealthBar(this);
            }
            Destroy(gameObject);
        }
        DamageTakenInfo damageTakenInfo = new DamageTakenInfo();
        damageTakenInfo.damage = info.damage;
        damageTakenInfo.currentHealth = currentHealth;
        damageTakenInfo.maxHealth = maxHealth;
        OnDamage?.Invoke(damageTakenInfo);
    }


    public void SetLevel(int level)
    {
        agent.speed = baseSpeed + (speedPerLevel * level);
        maxHealth = basehealth + (healthPerLevel * level);
        currentHealth = maxHealth;
        this.level = level;
    }


    void Start()
    {
        if(healthBar)
        {
            FindObjectOfType<UIEnemyHealthBars>().CreateHealthBar(this);
        }
        if(level == -1)
        {
            SetLevel(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        squad = GameplayController.Instance.SquadInstance;
        if (squad != null && agent.enabled)
        {
            Transform target = null;
            float closestDistance = 0f;
            for(int i = 0; i < squad.Units.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, squad.Units[i].transform.position);
                if(closestDistance < distance)
                {
                    target = squad.Units[i].transform;
                }
            }

            if(target == null)
            {
                return;
            }
            if (!agent.isStopped)
            {
                agent.SetDestination(squad.transform.position);
            }
            if (nextAttack < Time.time)
            {
                int count = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, attackColliders, attackMask);
                if (count > 0)
                {
                    nextAttack = Time.time + attackRate;
                    StartCoroutine(AttackCoroutine(attackColliders, count));
                }
            }
        }
    }

    
    public void Stop()
    {
        agent.enabled = false;
        StopAllCoroutines();
    }


    private IEnumerator AttackCoroutine(Collider[] colliders, int count)
    {
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(attackTime);
        for(int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.Damage(new DamageInfo { damage = attackDamage });
            }
        }
        agent.isStopped = false;
    }
}
