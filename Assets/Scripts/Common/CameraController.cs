using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : SingletonMono<CameraController>
{
    [SerializeField] Camera camera;
    Transform target;
    Vector3 currentVelocity;

    public Camera Camera => camera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, 0.1f);
        }
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
        transform.position = target.position;
    }

    public void Zoom(float newZoomValue, float duration, System.Action OnZoomEndCallback = null)
    {
        camera.transform.DOLocalMoveZ(newZoomValue,duration).OnComplete(()=> OnZoomEndCallback());
    }
}
