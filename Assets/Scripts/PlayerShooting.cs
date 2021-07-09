using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float shootRadius = 3f;
    [SerializeField] LayerMask attackMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] PlayerAnimation playerAnimation;
    [SerializeField] Weapon[] weapons;


    Vector2 input;
    PlayerResources playerResources;

    Collider[] attackColliders = new Collider[50];

    void Start()
    {
        playerResources = GetComponent<PlayerResources>();
    }


    void Update()
    {
        int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, shootRadius, attackColliders, attackMask);

        Transform rightTarget = GameplayUtils.GetAttackTarget(1, collidersCount, transform, attackColliders);
        Transform leftTarget = GameplayUtils.GetAttackTarget(-1, collidersCount, transform, attackColliders);

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
}
