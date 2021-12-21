using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Helicopter : MonoBehaviour
{
    public event System.Action OnArrived;

    [SerializeField] float yValue = 10f;
    [SerializeField] Transform endPoint;

    [SerializeField] float upSpeed = 2f;
    [SerializeField] float awaySpeed = 3f;
    [SerializeField] float rotateSpeed = 80f;
    [SerializeField] float waitTime = 1.5f;
    Vector3 beginPoint;    

    public void FlyAway()
    {       
        beginPoint = transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(waitTime);
        sequence.Append(transform.DOMoveY(yValue, yValue/upSpeed).SetEase(Ease.InCubic));
        Vector3 dir = endPoint.position.SetY(0) - transform.position.SetY(0);
        float dist = dir.magnitude;
        dir.Normalize();
        Quaternion rot = Quaternion.LookRotation(dir, transform.eulerAngles);        
        float eularDif = Mathf.DeltaAngle(transform.eulerAngles.y, rot.eulerAngles.y);        
        sequence.Append(transform.DORotate(new Vector3(0f, eularDif, 0f), Mathf.Abs(eularDif) / rotateSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
        sequence.Append(transform.DOMove(endPoint.position.SetY(yValue), dist/awaySpeed).SetEase(Ease.Linear));
        sequence.AppendCallback(() => gameObject.SetActive(false));
    }

    public void FlyBack()
    {
        transform.DOKill(false);
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        Vector3 dir = beginPoint.SetY(0) - endPoint.position.SetY(0);
        float dist = dir.magnitude;
        dir.Normalize();
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rot;
        sequence.Append(transform.DOMove(beginPoint.SetY(yValue), dist / awaySpeed));
        sequence.Append(transform.DOMoveY(beginPoint.y, yValue / upSpeed));
        sequence.AppendCallback(() => OnArrived?.Invoke());
    }


}
