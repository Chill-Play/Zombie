using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Constructive : MonoBehaviour
{
    Construction construction;

    public bool CanConstruct => !construction.Constructed && !construction.LockConstruction;

    private void Awake()
    {
        construction = GetComponent<Construction>();
    }

    public void Construct(float value)
    {
        construction.Construct(value);
    }
}
