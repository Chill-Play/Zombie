using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapArea : MonoBehaviour
{
    public static event System.Action OnGlobalMapAreaEnter;
    public static event System.Action OnGlobalMapAreaExit;

    private void Awake()
    {
        gameObject.SetActive(false);
        MapController.Instance.OnMapComplited += Instance_OnMapComplited;
    }

    private void Instance_OnMapComplited()
    {
        gameObject.SetActive(true);
        MapController.Instance.OnMapComplited -= Instance_OnMapComplited;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            OnGlobalMapAreaEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            OnGlobalMapAreaExit?.Invoke();
        }
    }
}
