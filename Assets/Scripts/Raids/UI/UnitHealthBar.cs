using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{ 
    void OnEnable()
    {
        FindObjectOfType<UIUnitHealthBars>().CreateHealthBar(this);
    }

    
    void OnDisable()
    {
        UIUnitHealthBars bars = FindObjectOfType<UIUnitHealthBars>();
        if (bars != null)
        {
            bars.RemoveHealthBar(this);
        }
    }
}
