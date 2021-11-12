using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class ZombieBreaksthroughToPlayer : MonoBehaviour
{
    UnitMovement unitMovement;
    UnitMeleeFighting meleeFighting;
    ZombieDetectConstructions detectConstructions;
    float nextUpdateTime;
    Squad squad;
    [SerializeField] ZombiesTarget zombiesTarget;

    private void Awake()
    {
        meleeFighting = GetComponent<UnitMeleeFighting>();
        unitMovement = GetComponent<UnitMovement>();
        detectConstructions = GetComponent<ZombieDetectConstructions>();
    }

    private void Start()
    {
        squad = FindObjectOfType<Squad>();
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > nextUpdateTime)
        {
            Transform targetTransform = transform;

            if (zombiesTarget != null && !zombiesTarget.enabled)
            {
                zombiesTarget = null;
                meleeFighting.SetTarget(null);
            }
            if (zombiesTarget == null && detectConstructions.Target != null)
            {
                zombiesTarget = detectConstructions.Target;
                zombiesTarget.AddZombie(gameObject);
                meleeFighting.SetTarget(zombiesTarget);
            }
            if (zombiesTarget != null && Vector3.Distance(transform.position,squad.transform.position) >= Vector3.Distance(transform.position, zombiesTarget.transform.position))
            {
                targetTransform = zombiesTarget.transform;
            }
            else if (squad != null)
            {
                targetTransform = squad.transform;
            }

            if (!meleeFighting.Attacking)
            {
                nextUpdateTime = Time.timeSinceLevelLoad + Random.Range(0.2f, 0.6f);
                unitMovement.MoveTo(targetTransform.position);
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
