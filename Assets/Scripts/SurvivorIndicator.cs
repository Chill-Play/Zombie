using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorIndicator : MonoBehaviour
{
    [SerializeField] Transform arrow;
    [SerializeField] Transform circle;
    [SerializeField] float circleRotationSpeed;
    [SerializeField] float arrowMovementSpeed;
    [SerializeField] float arrowMovementRange;
    [SerializeField] float arrowRotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        circle.transform.Rotate(Vector3.up * Time.deltaTime * circleRotationSpeed, Space.Self);
        arrow.transform.Rotate(Vector3.up * Time.deltaTime * arrowRotationSpeed, Space.Self);
        float t = Time.time * arrowMovementSpeed;
        arrow.transform.Translate(Vector3.up * Mathf.Sin(t) * arrowMovementRange * Time.deltaTime);
    }
}
