using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct PlayerDamageInfo
{
    public float damage;
    public float currentHealth;
    public float maxHealth;
}


public class Player : MonoBehaviour, IInputReceiver
{
    public event System.Action<PlayerDamageInfo> OnTakeDamage;
    public event System.Action OnDead;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] float angularSpeed;
    [SerializeField] float shootRadius = 3f;
    [SerializeField] LayerMask attackMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Weapon[] weapons;

    [SerializeField] float health = 100;

    Vector2 input;
    float currentHealth = 0f;
    PlayerResources playerResources;

    public Vector3 Velocity => agent.velocity;
    public bool InputActive => input.magnitude > Mathf.Epsilon;

    Collider[] attackColliders = new Collider[50];

    void Start()
    {
        currentHealth = health;
        playerResources = GetComponent<PlayerResources>();
    }


    void Update()
    {
        agent.velocity = new Vector3(input.x, 0f, input.y) * agent.speed;

        int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, shootRadius, attackColliders, attackMask);

        Transform rightTarget = GetAttackTarget(1, collidersCount);
        Transform leftTarget = GetAttackTarget(-1, collidersCount);

        if (rightTarget == null && leftTarget == null)
        {
            playerAnimation.HandIKActive = false;
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].StopFire();
            }
        }
        else
        {
            if (!playerAnimation.HandIKActive)
            {
                playerAnimation.HandIKActive = true;
            }
            playerAnimation.LeftIKTarget = (leftTarget != null ) ? leftTarget.position : rightTarget.position;
            playerAnimation.RightIKTarget = (rightTarget != null) ? rightTarget.position : leftTarget.position;

            weapons[0].StartFire((leftTarget != null) ? leftTarget : rightTarget);
            weapons[1].StartFire((rightTarget != null) ? rightTarget : leftTarget);
        }

        Vector3 lookLocation = Vector3.zero;
        int lookLocations = 0;

        if(rightTarget != null)
        {
            lookLocation += rightTarget.position;
            lookLocations++;
        }
        if(leftTarget != null)
        {
            lookLocation += leftTarget.position;
            lookLocations++;
        }
        playerResources.CanDigResources = rightTarget == null && leftTarget == null;
        lookLocation /= lookLocations;

        lookLocation.y = transform.position.y;

        if (lookLocations > 0)
        {
            transform.forward = lookLocation - transform.position;
        }
        else
        {
            Vector3 direction = new Vector3(input.x, 0f, input.y);
            if (direction.magnitude > 0f)
            {
                transform.forward = new Vector3(input.x, 0f, input.y);
            }
        }
    }


    public Transform GetAttackTarget(int side, int collidersCount)
    {
        float minDistance = Mathf.Infinity;
        Transform closest = null;
        for (int i = 0; i < collidersCount; i++)
        {
            if (attackColliders[i] == null)
                break;
            Transform target = attackColliders[i].transform;
            float angle = Vector3.SignedAngle(transform.forward, target.position - transform.position, Vector3.up);
            float distance = Vector3.Distance(transform.position, target.position);
            if (!Physics.Linecast(transform.position + Vector3.up * 0.5f, target.transform.position + Vector3.up * 0.5f, obstacleMask))
            {
                if (minDistance > distance && Mathf.Sign(angle) == side)
                {
                    minDistance = distance;
                    closest = target.transform;
                }
            }
        }
        return closest;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        PlayerDamageInfo info = new PlayerDamageInfo()
        {
            damage = damage,
            maxHealth = health,
            currentHealth = currentHealth,
        };

        OnTakeDamage?.Invoke(info);
        if(currentHealth <= 0f)
        {
            OnDead?.Invoke();
            gameObject.SetActive(false);
        }
    }



    public void ToFightMode()
    {
        //GetComponent<PlayerResources>().enabled = false;
    }


    public void SetInput(Vector2 input)
    {
        this.input = input;
    }

}
