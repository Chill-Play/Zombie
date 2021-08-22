using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public SubjectId subjectId;

        public List<Behaviour> components = new List<Behaviour>();
    }

    [SerializeField] List<State> states = new List<State>();

    State currentState;

    public SubjectId CurrentStateId => currentState == null? null : currentState.subjectId;

    public void ToState(SubjectId subjectId)
    {
        if (currentState != null)
        {
            DisableState(currentState);
        }

        currentState = GetState(subjectId);

        if (currentState != null)
        {
            EnableState(currentState);
        }
        else
        {
            Debug.LogError("State not found");
        }
    }

    State GetState(SubjectId subjectId)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].subjectId == subjectId)
            {
                return states[i];
            }
        }
        return null;
    }

    void EnableState(State state)
    {
        for (int i = 0; i < state.components.Count; i++)
        {
            state.components[i].enabled = true;
        }
    }

    void DisableState(State state)
    {
        for (int i = 0; i < state.components.Count; i++)
        {
            state.components[i].enabled = false;
        }
    }
}
