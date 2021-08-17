using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    public event System.Action<Collider> OnTriggerEnterEvent;
    public event System.Action<Collider> OnTriggerExitEvent;


    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }


    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }
}
