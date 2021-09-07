using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialColiderEnabler : MonoBehaviour
{
    [SerializeField] ConditionTrigger conditionTrigger;
    [SerializeField] List<Collider> colliders = new List<Collider>();


    private void Awake()
    {
        if (conditionTrigger != null)
        {
            conditionTrigger.OnTrigger += ConditionTrigger_OnTrigger;
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].enabled = false;
            }
        }
    }

    private void ConditionTrigger_OnTrigger()
    {
        conditionTrigger.OnTrigger -= ConditionTrigger_OnTrigger;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = true;
        }
    }
}
