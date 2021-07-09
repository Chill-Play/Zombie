using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMovement))]
public class UnitPlayerInput : MonoBehaviour, IInputReceiver
{
    [SerializeField] UnitMovement unitMovement;

    public void SetInput(Vector2 input)
    {
        unitMovement.Input = input;
    }
}
