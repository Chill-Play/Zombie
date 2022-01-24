using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventListener : MonoBehaviour
{
    public event System.Action<Collision> OnCollisionEnterEvent;
    public event System.Action<Collision> OnCollisionExitEvent;
    public event System.Action<Collider> OnTriggerEnterEvent;
    public event System.Action<Collider> OnTriggerExitEvent;
    
    [SerializeField] float lockTimer = 0f;

    bool lockTrigger = false;

    private void OnEnable()
    {
        if (lockTimer > 0f)
        {
            lockTrigger = true;
            StartCoroutine(LockTimer());
        }
    }

    IEnumerator LockTimer()
    {
        yield return new WaitForSeconds(lockTimer);
        lockTrigger = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!lockTrigger)
        OnCollisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!lockTrigger)
            OnCollisionExitEvent?.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!lockTrigger)
            OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!lockTrigger)
            OnTriggerExitEvent?.Invoke(other);
    }

}
