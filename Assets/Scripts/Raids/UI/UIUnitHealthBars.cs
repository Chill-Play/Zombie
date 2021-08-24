using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitHealthBars : MonoBehaviour
{
    [SerializeField] UIUnitHealthBar healthBarPrefab;
    Dictionary<UnitHealthBar, UIUnitHealthBar> healthBars = new Dictionary<UnitHealthBar, UIUnitHealthBar>();


    public void CreateHealthBar(UnitHealthBar enemy)
    {
        UIUnitHealthBar instance = Instantiate(healthBarPrefab, transform);
        instance.Setup(enemy, 3.5f);
        healthBars.Add(enemy, instance);
    }


    public void RemoveHealthBar(UnitHealthBar enemy)
    {
        healthBars[enemy].Remove();
        healthBars.Remove(enemy);
    }
}
