using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputJoystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event System.Action OnBeginDragEvent;
    public event System.Action OnEndDragEvent;


    [SerializeField] float padRadius = 200f;
    [SerializeField] Component target;

    public IInputReceiver InputReceiver;

    private bool _isMoving;
     
    public Vector2 StartPosition { get; set; }
    public Vector2 CurrentPosition { get; set; }


    public void OnBeginDrag(PointerEventData eventData)
    {
        StartPosition = eventData.pressPosition;
        _isMoving = true;
        CurrentPosition = StartPosition;
        OnBeginDragEvent?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isMoving = false;
        OnEndDragEvent?.Invoke();   
    }

    public void OnDrag(PointerEventData eventData)
    {
        CurrentPosition = eventData.position;
    }

    void Update()
    {
        if(InputReceiver == null)
        {
            return;
        }
        if (_isMoving)
        {
            var delta = Vector2.ClampMagnitude(CurrentPosition - StartPosition, padRadius) / padRadius * transform.localScale.y;
            InputReceiver.SetInput(delta);
        }
        else
        {
            InputReceiver.SetInput(Vector2.zero);
        }
    }


    public void ResetInput()
    {
        _isMoving = false;
    }
}
