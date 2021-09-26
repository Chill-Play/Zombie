using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovementTester : MonoBehaviour
{
    [SerializeField] ZombieMovement movement;
    float nextUpdateTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > nextUpdateTime)
        {
            movement.MoveTo(transform.position);
            nextUpdateTime = Time.timeSinceLevelLoad + Random.Range(0.5f, 1f);
        }
    }
}
