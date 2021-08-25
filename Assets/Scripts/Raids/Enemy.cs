using DG.Tweening;
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



public class Enemy : MonoBehaviour
{
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float speedPerLevel = 1f;

    [SerializeField] SubjectId wanderingState;
    [SerializeField] SubjectId aggressiveState;
    [SerializeField] SubjectId deadState;

    StateController stateController;
    int level = -1;
    Squad squad;
    ZombieAgroSequence zombieAgroSequence;

    public bool IsDead => stateController.CurrentStateId == deadState;


    public void SetLevel(int level)
    {
        //agent.speed = baseSpeed + (speedPerLevel * level);
        this.level = level;
    }

    private void OnDisable()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }


    private void OnEnable()
    {
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }


    private void Awake()
    {
        stateController = GetComponent<StateController>();
        zombieAgroSequence = GetComponent<ZombieAgroSequence>();
    }


    void Start()
    {
        if (stateController.CurrentStateId != aggressiveState)
        {
            stateController.ToState(wanderingState);
        }
        GetComponent<UnitHealth>().OnDead += Enemy_OnDead;
        GetComponent<UnitHealth>().OnDamage += Enemy_OnDamage;
        if (level == -1)
        {
            SetLevel(1);
        }
    }

    private void Enemy_OnDamage(DamageTakenInfo obj)
    {
        StartAggro();
    }


    private void Enemy_OnDead(EventMessage<Empty> obj)
    {
        stateController.ToState(deadState);
        GetComponent<UnitMovement>().StopMoving();
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;
        var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        renderer.material.DOColor(Color.gray, "_MainColor", 0.5f);
    }


    // Update is called once per frame
    void Update()
    {
        squad = GameplayController.Instance.SquadInstance;
        if (Vector3.Distance(squad.transform.position, transform.position) < 6f)
        {
            StartAggro();
        }
    }

    private void StartAggro()
    {
        if (stateController.CurrentStateId == wanderingState && !zombieAgroSequence.IsPlaying)
        {
            GoAggressive();
        }
    }

    public void GoAggressive()
    {
        float rand = Random.Range(0f, 100f);
        if (rand < 30f)
        {
            zombieAgroSequence.Play(() =>
            {
                stateController.ToState(aggressiveState);
            });
        }
        else
        {
            stateController.ToState(aggressiveState);
        }
    }

    public void Stop()
    {
        //agent.enabled = false;
        StopAllCoroutines();
    }
}
