using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMovement))]
public class UnitPlayerInput : MonoBehaviour, IInputReceiver
{
    [SerializeField] UnitMovement unitMovement;

    InputPanel inputPanel;

    private void Awake()
    {
        inputPanel = InputPanel.Instance;
        inputPanel.OnDisableInput += InputPanel_OnDisableInput;
    }

    private void InputPanel_OnDisableInput()
    {
        unitMovement.Input = Vector2.zero;
    }

    public void UpdateInput(Vector2 input)
    {
        input.Normalize();
        unitMovement.Input = input;
    }
}
