using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateArea : MonoBehaviour
{
    [SerializeField] SceneReference stateBase;

    public void LoadStateBase()
    {
        StatesLoader.Instance.LoadState(stateBase);
    }
}
