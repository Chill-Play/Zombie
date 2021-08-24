using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovementTester : MonoBehaviour
{
    [SerializeField] ZombieMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.MoveTo(transform.position);
    }
}
