using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMeleeFighting : MonoBehaviour
{
    public event System.Action OnAttack;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float attackDamage = 1f;
    [SerializeField] float attackRate = 1.5f;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackTime = 0.5f;
    [SerializeField] LayerMask attackMask;

    Squad squad;
    float nextAttack;
    bool attackStarted;
    Collider[] attackColliders = new Collider[5];
    Coroutine attackCoroutine;
    public bool Attacking { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        squad = GameplayController.Instance.SquadInstance;
        if (squad != null && agent.enabled)
        {
            Transform target = null;
            float closestDistance = 0f;
            for (int i = 0; i < squad.Units.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, squad.Units[i].transform.position);
                if (closestDistance < distance)
                {
                    target = squad.Units[i].transform;
                }
            }
            if (nextAttack < Time.time)
            {
                int count = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, attackColliders, attackMask);
                if (count > 0 && attackCoroutine == null)
                {
                    nextAttack = Time.time + attackRate;
                    attackCoroutine = StartCoroutine(AttackCoroutine(attackColliders, count));
                }
            }
        }
    }




    private IEnumerator AttackCoroutine(Collider[] colliders, int count)
    {
        OnAttack?.Invoke();
        //agent.isStopped = true;
        Attacking = true;
        GetComponent<ZombieMovement>().StopMoving();
        yield return new WaitForSeconds(attackTime);
        for (int i = 0; i < count; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.Damage(new DamageInfo { damage = attackDamage });
            }
        }
        Attacking = false;
        attackCoroutine = null;
        //agent.isStopped = false;
    }
}
