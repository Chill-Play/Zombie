using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingElement : MonoBehaviour
{
    [SerializeField] Vector3 rotation = new Vector3(0f, 1f, 0f);
    [SerializeField] bool instantRotation = false;

    Tween rotationTween;

    private void Start()
    {
        if (instantRotation)
        {
            StartRotation();
        }
    }

    public void StartRotation()
    {
        rotationTween.Kill(true);
        rotationTween = transform.DOLocalRotate(rotation, 1f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
    }

    public void StopRotation()
    {
        rotationTween.Kill(true);        
    }

}
