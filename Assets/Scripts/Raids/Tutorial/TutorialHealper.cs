using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHealper : MonoBehaviour
{
    public event System.Action OnEscapeTrigger;

    [SerializeField] ConditionTrigger escapeTrigger;


    private void Awake()
    {
        if (escapeTrigger != null)
        {
            escapeTrigger.OnTrigger += EscapeTrigger_OnTrigger;
        }
    }

    private void EscapeTrigger_OnTrigger()
    {
        FindObjectOfType<SpawnPoint>().IsReturningToBase = true;
        OnEscapeTrigger?.Invoke();
    }
}
