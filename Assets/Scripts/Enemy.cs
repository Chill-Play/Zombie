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

    int level = -1;
    Squad squad;


    public void SetLevel(int level)
    {
        //agent.speed = baseSpeed + (speedPerLevel * level);
        this.level = level;
    }


    void Start()
    {
        if(level == -1)
        {
            SetLevel(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        squad = GameplayController.Instance.SquadInstance;
        if (squad != null)
        {
            GetComponent<UnitMovement>().MoveTo(squad.transform.position);
        }
    }

    
    public void Stop()
    {
        //agent.enabled = false;
        StopAllCoroutines();
    }
}
