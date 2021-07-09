using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputJoystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] float padRadius = 200f;

    public IInputReceiver InputReceiver;

    private Vector2 _startPosition;
    private bool _isMoving;
    private Vector2 _currentPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = eventData.pressPosition;
        _isMoving = true;
        _currentPosition = _startPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isMoving = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentPosition = eventData.position;
    }

    void Update()
    {
        if(InputReceiver == null)
        {
            return;
        }
        if (_isMoving)
        {
            var delta = Vector2.ClampMagnitude(_currentPosition - _startPosition, padRadius) / padRadius * transform.localScale.y;
            InputReceiver.SetInput(delta);
        }
        else
        {
            InputReceiver.SetInput(Vector2.zero);
        }
    }
}
