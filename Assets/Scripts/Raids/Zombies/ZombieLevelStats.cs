using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieLevelStats : MonoBehaviour
{
    [SerializeField] float speedPerLevel;
    [SerializeField] float attackDamagePerLevel;
    [SerializeField] float healthPerLevel;

    [SerializeField] float speedPerGeneration;
    [SerializeField] float attackDamagePerGeneration;
    [SerializeField] float healthPerGeneration;

    [SerializeField] UnitHealth unitHealth;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] UnitMeleeFighting meleeFighting;


    public void SetLevel(int level, int generation)
    {
        meleeFighting.AttackBuff += (level * attackDamagePerLevel) + (generation * attackDamagePerGeneration);
        unitHealth.CurrentHealth += (level * healthPerLevel) + (generation * healthPerGeneration);
        agent.speed += (level * speedPerLevel) + (generation * speedPerGeneration);
    }
}
