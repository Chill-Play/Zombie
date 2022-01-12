using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : SingletonMono<CameraController>
{
    [SerializeField] Camera camera;
    [SerializeField] Transform camRotation;
    Transform target;
    Vector3 currentVelocity;

    public Camera Camera => camera;

    Transform lastTarget;
    Quaternion defaultRotatiom;
    float defaultZ;
    float reachingTime = 0.1f;
    System.Action OnReachingCallback;

    private void Awake()
    {
        if (camRotation != null)
        {
            defaultRotatiom = camRotation.localRotation;
        }
        defaultZ = camera.transform.localPosition.z;
    }

    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {   
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, reachingTime);
            if (Vector3.Distance(transform.position, target.position) < 0.2f && OnReachingCallback != null)
            {
                OnReachingCallback?.Invoke();
            }
        }
    }


    public void SetTarget(Transform target, float reachingTime = 0.1f, System.Action OnReachingCallback = null)
    {        
        lastTarget = target;
        this.target = target;
        this.reachingTime = reachingTime;
        this.OnReachingCallback = () => { OnReachingCallback?.Invoke(); OnReachingCallback = null; }; /// OMG    
    }

    public void Zoom(float newZoomValue, float duration, System.Action OnZoomEndCallback = null)
    {
        camera.transform.DOLocalMoveZ(newZoomValue,duration).OnComplete(()=> OnZoomEndCallback());
    }

    public void SetCameraPoint(Transform point, float camZ)
    {
        target = null;
        transform.position = point.position;
        camRotation.localRotation = point.rotation;
        camera.transform.localPosition = Vector3.zero.SetZ(camZ);
    }

    public void ResetCameraPoint()
    {
        camera.transform.localPosition = Vector3.zero.SetZ(defaultZ);
        target = lastTarget;
        camRotation.localRotation = defaultRotatiom;
        
    }
}
