using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CivilianAnimation), typeof(UnitMovement))]
public class Civilian : MonoBehaviour
{   
    [SerializeField] Vector2 idleTime = new Vector2(3f, 5f);
    [SerializeField] List<Transform> points = new List<Transform>();

    [SerializeField] UnitMovement unitMovement;
    [SerializeField] CivilianAnimation civilianAnimation;
    [SerializeField] float rotationSpeed = 40f;
    int idx = 0;

    public void GoIdle()
    {   
        float idleTimer = Random.Range(idleTime.x, idleTime.y);
        StartCoroutine(Idle(idleTimer));
    }

    IEnumerator Idle(float time)
    {
        civilianAnimation.GoIdle();
        yield return new WaitForSeconds(time);
        TryMoveToNextPoint();
    }

    void TryMoveToNextPoint()
    {
        if (points.Count > 0)
        {
            idx++;
            if (idx >= points.Count)
            {
                idx = 0;
            }
            civilianAnimation.GoWalk();
            StartCoroutine(GoToPoint(points[idx], () => transform.DORotate(points[idx].eulerAngles, 0.5f).OnComplete(() => GoIdle())));
        }
    }

    IEnumerator GoToPoint(Transform point, System.Action callback = null)
    {        
        unitMovement.MoveTo(point.position);
        while (Vector3.Distance(transform.position, point.position) > 0.2f)
        {
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }

    public void GoToBuilding(CivilianBuilding civilianBuilding)
    {
        StopAllCoroutines();
        civilianAnimation.GoRun();
        StartCoroutine(GoToPoint(civilianBuilding.DoorPoint, () => Destroy(gameObject)));
    }

    public void GoDance(Transform point)
    {        
        civilianAnimation.GoRun();
        float deltaRot = (point.eulerAngles - transform.eulerAngles).magnitude;
        StartCoroutine(GoToPoint(point, () => transform.DORotate(point.eulerAngles, deltaRot/rotationSpeed).OnComplete(() => civilianAnimation.GoDance())));
    }

}
