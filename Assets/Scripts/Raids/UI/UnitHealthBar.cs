using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] SubjectId uiHealthBarId;
    [SerializeField] GameObject damagable;

    public GameObject Damagable => damagable;
    public SubjectId UIHealthBarId => uiHealthBarId;

    void Start()
    {
        UIUnitHealthBars.Instance.CreateHealthBar(this);
    }

    
    void OnDisable()
    {
        UIUnitHealthBars bars = UIUnitHealthBars.Instance;
        if (bars != null)
        {
            bars.RemoveHealthBar(this);
        }
    }
}
