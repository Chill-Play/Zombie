using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] float rotateSpead = 1f;
    [SerializeField] UnitShooting unitShooting;
    [SerializeField] UnitTargetDetection targetDetection;  
    [SerializeField] Transform modelPivot;    


    private void Awake()
    {
        unitShooting = GetComponent<UnitShooting>();      
    }

    private void Update()
    {
        if (targetDetection.Target != null)
        {
            RotateToTarget();
        }
    }

    void RotateToTarget()
    {
        Vector3 direction = targetDetection.Target.transform.position - modelPivot.position;
        direction.y = 0;

        direction.Normalize();
        modelPivot.rotation = Quaternion.RotateTowards(modelPivot.rotation, Quaternion.LookRotation(direction), rotateSpead * Time.deltaTime);
        unitShooting.AllowShooting = Vector3.Angle(modelPivot.transform.forward, direction) < 15f;
    }
}
