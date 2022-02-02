using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class ZombieBreaksthroughToPlayer : MonoBehaviour
{
    UnitMovement unitMovement;
    UnitFighting unitFighting;
    ZombieDetectConstructions detectConstructions;
    float nextUpdateTime;
    Squad squad;
    ZombiesTarget zombiesTarget;
    [SerializeField] private bool OhShitHereWeGoAgain = false; //refactor

    private void Awake()
    {
        unitFighting = GetComponent<UnitFighting>();
        unitMovement = GetComponent<UnitMovement>();
        detectConstructions = GetComponent<ZombieDetectConstructions>();
    }

    private void Start()
    {
        squad = Squad.Instance;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > nextUpdateTime)
        {
            Transform targetTransform = transform;

            if (zombiesTarget != null && !zombiesTarget.enabled)
            {
                zombiesTarget = null;
                unitFighting.SetTarget(null);
            }
            if (zombiesTarget == null && detectConstructions.Target != null)
            {
                zombiesTarget = detectConstructions.Target;
                zombiesTarget.AddZombie(gameObject);
                unitFighting.SetTarget(zombiesTarget);
            }
            if (zombiesTarget != null && Vector3.Distance(transform.position,squad.transform.position) >= Vector3.Distance(transform.position, zombiesTarget.transform.position))
            {
                targetTransform = zombiesTarget.transform;
            }
            else if (squad != null)
            {
                targetTransform = squad.transform;
            }

            if (!unitFighting.Attacking)
            {
                nextUpdateTime = Time.timeSinceLevelLoad + Random.Range(0.2f, 0.6f);
                unitMovement.MoveTo(targetTransform.position);
            }
            if (unitFighting.Attacking && !OhShitHereWeGoAgain)
            {
                unitMovement.StopMoving();
            }
        }
    }

    private void OnDisable()
    {
        if (zombiesTarget != null)
        {
            zombiesTarget.RemoveZombie(gameObject);
        }
    }

}
