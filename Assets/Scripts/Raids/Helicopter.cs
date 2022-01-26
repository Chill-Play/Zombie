using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Helicopter : SingletonMono<Helicopter>
{
    public event System.Action OnArrived;

    [SerializeField] float yValue = 10f;
    [SerializeField] Transform endPoint;

    [SerializeField] AnimationCurve upCurve;
    [SerializeField] AnimationCurve awayCurve;
    [SerializeField] float upSpeed = 2f;
    [SerializeField] float awaySpeed = 3f;
    [SerializeField] float rotateSpeed = 80f;
    [SerializeField] float waitTime = 1.5f;
    Vector3 beginPoint;    

    public void FlyAway()
    {       
        beginPoint = transform.position;       
        StartCoroutine(FlyAwayCoroutine());
    }

    IEnumerator FlyAwayCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        float beginDistanceUp = Vector3.Distance(transform.position, transform.position.SetY(yValue));
        float t = 0f;
        while (transform.position.y < yValue)
        {            
            ToPoint(transform.position.SetY(yValue), upSpeed, beginDistanceUp, t, upCurve);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Vector3 dir = endPoint.position.SetY(0) - transform.position.SetY(0);
        float dist = dir.magnitude;
        dir.Normalize();
        Quaternion rot = Quaternion.LookRotation(dir, transform.eulerAngles);
        float eularDif = Mathf.DeltaAngle(transform.eulerAngles.y, rot.eulerAngles.y);
        bool rotating = true;
        transform.DORotate(new Vector3(0f, eularDif, 0f), Mathf.Abs(eularDif) / rotateSpeed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() => rotating = false);
        while (rotating)
        {
            yield return new WaitForEndOfFrame();
        }

        float beginDistanceAway = Vector3.Distance(transform.position, endPoint.position.SetY(yValue));
        t = 0f;
        while (Vector3.Distance(transform.position, endPoint.position.SetY(yValue)) > 0.2f)
        {
            ToPoint(endPoint.position.SetY(yValue), awaySpeed, beginDistanceAway, t, awayCurve);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

    void ToPoint(Vector3 point, float speed, float beginDistance, float dt, AnimationCurve speedCurve)
    {
        Vector3 dir = point - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        float speedCoof = speedCurve.Evaluate(dt);
        float curveSpeed = speed * speedCoof;
        float delta = curveSpeed * Time.deltaTime;        
        if (delta > dist)
        {
            delta = dist;
        }
        transform.position += delta * dir;
    }

    public void FlyBack()
    {
        transform.DOKill(false);
        gameObject.SetActive(true);
        Vector3 dir = beginPoint.SetY(0) - endPoint.position.SetY(0);       
        dir.Normalize();
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = rot;
        StartCoroutine(FlyBackCoroutine());
    }

    IEnumerator FlyBackCoroutine()
    {
        float beginDistanceAway = Vector3.Distance(transform.position, beginPoint.SetY(yValue));
        float t = 0f;
        while (Vector3.Distance(transform.position, beginPoint.SetY(yValue)) > 0.2f)
        {
            ToPoint(beginPoint.SetY(yValue), awaySpeed, beginDistanceAway, t, awayCurve);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        float beginDistanceUp = Vector3.Distance(beginPoint, transform.position.SetY(yValue));
        t = 0f;
        while (transform.position.y > beginPoint.y)
        {
            ToPoint(beginPoint, upSpeed, beginDistanceUp, t, upCurve);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        OnArrived?.Invoke();
    }


}
