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
        Damagable.GetComponent<IDamagable>().OnDead += Damagable_OnDeath;
        UIUnitHealthBars.Instance.CreateHealthBar(this);       
    }

    
    void Damagable_OnDeath(EventMessage<Empty> empty)
    {
        UIUnitHealthBars bars = UIUnitHealthBars.Instance;
        if (bars != null)
        {
            bars.RemoveHealthBar(this);
        }
    }
}
