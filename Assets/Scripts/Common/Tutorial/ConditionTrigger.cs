using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTrigger : MonoBehaviour
{
    public event System.Action OnTrigger;

    public void InvokeEvent()
    {
        OnTrigger?.Invoke();
    }

}
