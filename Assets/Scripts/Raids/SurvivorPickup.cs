using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorPickup : MonoBehaviour
{
    [SerializeField] Unit unitPrefab;

    Squad squad;

    void Awake()
    {
        squad = FindObjectOfType<Squad>();
    }


    // Update is called once per frame
    void Pickup()
    {
        Unit instance = Instantiate(unitPrefab, transform.position, transform.rotation);
        squad.AddUnit(instance);
        Destroy(gameObject);
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
