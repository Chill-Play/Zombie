using System;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private int pointsCount = 2;
    [SerializeField] float animationSpeed = 1;
    private LineRenderer lineRenderer;
    private Vector2 offset;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //gameObject.transform.parent = null;
    }

    private void Update()
    {
        offset.x -= animationSpeed * Time.deltaTime;
        lineRenderer.material.SetTextureOffset("_MainTex",offset);
    }

    public void ShowTrajectory(Vector3 start, Vector3 end)
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        Vector3[] points = new Vector3[pointsCount];
        //lineRenderer.positionCount = points.Length;
        points[0] = start;
        points[pointsCount - 1] = end;
        lineRenderer.SetPositions(points);
    }
}