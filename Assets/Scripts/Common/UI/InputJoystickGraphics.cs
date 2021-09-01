using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputJoystickGraphics : MonoBehaviour
{
    [SerializeField] Image knob;
    [SerializeField] Image pad;
    [SerializeField] CanvasGroup group;
    [SerializeField] float knobBorderMargin;
    InputJoystick joystick;

    Tween fadeTween;
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }


    private void OnDisable()
    {
        joystick.OnBeginDragEvent -= Joystick_OnBeginDragEvent;
        joystick.OnEndDragEvent -= Joystick_OnEndDragEvent;
    }


    private void OnEnable()
    {
        joystick = GetComponent<InputJoystick>();
        group.alpha = 0.0f;
        joystick.OnBeginDragEvent += Joystick_OnBeginDragEvent;
        joystick.OnEndDragEvent += Joystick_OnEndDragEvent;
    }


    private void Joystick_OnEndDragEvent()
    {
        StartFadeTween(0f);
    }

    private void Joystick_OnBeginDragEvent()
    {
        StartFadeTween(1f);
        pad.transform.position = joystick.StartPosition;
    }


    void StartFadeTween(float target)
    {
        if(fadeTween != null)
        {
            fadeTween.Complete();
            fadeTween = null;
        }
        fadeTween = DOTween.To(() => group.alpha, (x) => group.alpha = x, target, 0.3f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var diff = joystick.CurrentPosition - joystick.StartPosition;
        var dir = diff.normalized;
        var magnitude = Mathf.Clamp(diff.magnitude, 0f, (pad.rectTransform.rect.width * canvas.scaleFactor / 2.0f) - (knob.rectTransform.rect.width * canvas.scaleFactor / 2.0f) - knobBorderMargin);
        knob.rectTransform.position = joystick.StartPosition + (dir * magnitude);
    }
}
