using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Repairable : MonoBehaviour
{
    Construction construction;

    public bool CanRepair => construction.Constructed;

    private void Awake()
    {
        construction = GetComponent<Construction>();
    }

    public void Repair(float value)
    {
        construction.Repair(value);
    }
}
