using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesTrigger : MonoBehaviour
{   
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SurvivorAI>() != null)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].GoAggressive();
            }
            Destroy(gameObject);
        }
    }
}
