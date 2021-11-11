using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesTarget : MonoBehaviour
{
    [SerializeField] int maxZombies = 5;   

    List<GameObject> enemies = new List<GameObject>();

    Construction construction;

    public bool CanBeAdded => enemies.Count < maxZombies;

    private void Awake()
    {
        construction = GetComponent<Construction>();
        if (!construction.Constructed)
        {
            enabled = false;
        }
        construction.OnBuild += Construction_OnBuild;
        construction.OnDead += Construction_OnDead;
    }

    private void Construction_OnDead(EventMessage<Empty> obj)
    {
        enabled = false;
    }

    private void Construction_OnBuild()
    {
        enabled = true;
    }

    public void AddZombie(GameObject enemy)
    {
        if(!enemies.Contains(enemy))
        enemies.Add(enemy);
    }

    public void RemoveZombie(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

}
