using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMeleeFighting : MonoBehaviour
{
    public event System.Action OnAttack;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] ZombieMovement zombieMovement;
    [SerializeField] float attackDamage = 1f;
    [SerializeField] float attackRate = 1.5f;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackTime = 0.5f;
    [SerializeField] float attackReset = 1.5f;
    [SerializeField] LayerMask attackMask;

    Squad squad;
    float nextAttack;
    bool attackStarted;
    Collider[] attackColliders = new Collider[5];
    Coroutine attackCoroutine;
    public bool Attacking { get; set; }
    public float AttackBuff { get; set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<UnitHealth>().OnDead += UnitMeleeFighting_OnDead;
    }

    private void Start()
    {
        squad = GameplayController.Instance.SquadInstance;
    }


    private void OnDisable()
    {
        GetComponent<UnitHealth>().OnDead -= UnitMeleeFighting_OnDead;
    }



    // Update is called once per frame
    void Update()
    {

        if((transform.position - squad.transform.position).sqrMagnitude > 50)
        {
            return;
        }
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
                    attackCoroutine = StartCoroutine(AttackCoroutine(attackColliders));
                }
            }
        }
    }




    private IEnumerator AttackCoroutine(Collider[] colliders)
    {
        OnAttack?.Invoke();
        //agent.isStopped = true;
        Attacking = true;
        zombieMovement.StopMoving();
        float t = 0f;
        Vector3 targetLook = colliders[0].transform.position - transform.position;
        targetLook.y = 0.0f;
        targetLook.Normalize();
        while(t < 1.0f)
        {
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime / attackTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetLook), agent.angularSpeed * Time.fixedDeltaTime);
        }
        int count = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, colliders, attackMask);
        IDamagable closestDamagable = null;
        var closestDistance = Mathf.Infinity;
        for (int i = 0; i < count; i++)
        {
            var targetCollider = colliders[i];
            IDamagable damagable = targetCollider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                var distance = Vector3.Distance(transform.position, targetCollider.transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamagable = damagable;
                }
            }
        }
        if(closestDamagable != null)
        {
            closestDamagable.Damage(new DamageInfo { damage = attackDamage });
        }
        yield return new WaitForSeconds(attackReset);
        Attacking = false;
        attackCoroutine = null;
        //agent.isStopped = false;
    }


    private void UnitMeleeFighting_OnDead(EventMessage<Empty> obj)
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}
