using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] SubjectId uiHealthBarId;
    [SerializeField] GameObject damagable;
    
    public GameObject Damagable => damagable;
    public SubjectId UIHealthBarId => uiHealthBarId;
    private UnitHealth unitHealth;

    void Start()
    {
        unitHealth = GetComponent<UnitHealth>();
        UIUnitHealthBars.Instance.CreateHealthBar(this);
        unitHealth.OnDead += OnDeath;
    }

    
    void OnDeath(EventMessage<Empty> empty)
    {
        UIUnitHealthBars bars = UIUnitHealthBars.Instance;
        if (bars != null)
        {
            bars.RemoveHealthBar(this);
        }
    }
}
