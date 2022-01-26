using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorPickup : MonoBehaviour
{
    public event System.Action<SurvivorPickup> OnPickup;

    [SerializeField] Unit unitPrefab;

    Squad squad;
    bool pickuped = false;

    void Awake()
    {
        squad = Squad.Instance;
    }


    // Update is called once per frame
    void Pickup()
    {
        if (!pickuped)
        {
            Unit instance = Instantiate(unitPrefab, transform.position, transform.rotation);
            squad.AddUnit(instance);
            OnPickup?.Invoke(this);
            Destroy(gameObject);
            pickuped = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        UnitMovement unit = other.GetComponent<UnitMovement>();
        if(unit != null)
        {
            Pickup();
        }
    }
}
