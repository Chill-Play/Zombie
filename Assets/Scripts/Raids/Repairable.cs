using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Repairable : MonoBehaviour
{
    [SerializeField] ConstructionHealth constructionHealth;
    Construction construction;

    public bool CanRepair => construction.Constructed && constructionHealth.CurrentHealth < constructionHealth.Health && !construction.LockConstruction;

    private void Awake()
    {
        construction = GetComponent<Construction>();
    }

    public void Repair(float value)
    {
        construction.Repair(value);
    }
}
