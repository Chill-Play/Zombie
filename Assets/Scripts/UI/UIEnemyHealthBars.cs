using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHealthBars : MonoBehaviour
{
    [SerializeField] EnemyHealthBar healthBarPrefab;
    Dictionary<Enemy, EnemyHealthBar> healthBars = new Dictionary<Enemy, EnemyHealthBar>();


    public void CreateHealthBar(Enemy enemy)
    {
        EnemyHealthBar instance = Instantiate(healthBarPrefab, transform);
        instance.Appear(enemy, 3.5f);
        healthBars.Add(enemy, instance);
    }


    public void RemoveHealthBar(Enemy enemy)
    {
        healthBars[enemy].Remove();
        healthBars.Remove(enemy);
    }
}
