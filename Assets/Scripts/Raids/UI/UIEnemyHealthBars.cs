using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHealthBars : MonoBehaviour
{
    [SerializeField] UIEnemyHealthBar healthBarPrefab;
    Dictionary<EnemyHealthBar, UIEnemyHealthBar> healthBars = new Dictionary<EnemyHealthBar, UIEnemyHealthBar>();


    public void CreateHealthBar(EnemyHealthBar enemy)
    {
        UIEnemyHealthBar instance = Instantiate(healthBarPrefab, transform);
        instance.Setup(enemy, 3.5f);
        healthBars.Add(enemy, instance);
    }


    public void RemoveHealthBar(EnemyHealthBar enemy)
    {
        healthBars[enemy].Remove();
        healthBars.Remove(enemy);
    }
}
