using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractivePoint : MonoBehaviour
{    
    public struct WorkingPoint
    {
        public Transform transform;
        public int index;

        public WorkingPoint(Transform transform, int index)
        {
            this.transform = transform;
            this.index = index;
        }
    }

    [SerializeField] float radius =3f;
    [SerializeField] int amountToSpawn = 12;
    [SerializeField] float entityRadius = 0.5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject debugWorkingPointPrefab;
    [SerializeField] List<Transform> workingPoint = new List<Transform>();

    bool[] workingPointFree;   

    private void Start()
    {
        workingPointFree = new bool[workingPoint.Count];
        for (int i = 0; i < workingPoint.Count; i++)
        {
            workingPoint[i].gameObject.SetActive(false);
            workingPointFree[i] = true;
        }
    }

    public WorkingPoint GetFreePoint(Vector3 position, UnitMovement unitMovement)
    {
        int idx = -1;
        float minDist = float.MaxValue;
        for (int i = 0; i < workingPoint.Count; i++)
        {
            float dist = Vector3.Distance(position, workingPoint[i].position);
            if (workingPointFree[i] && dist < minDist && unitMovement.CanReachDestination(workingPoint[i].position))
            {
                minDist = dist;
                idx = i;
            }
        }

        if (idx != -1)
        {            
            return new WorkingPoint(workingPoint[idx], idx);
        }

        return new WorkingPoint(null, -1);
    }

    public void TakePoint(WorkingPoint point)
    {
        workingPointFree[point.index] = false;
    }

    public void FreePoint(WorkingPoint workingPoint)
    {       
        workingPointFree[workingPoint.index] = true;
    }

    public bool HasFreePoint()
    {
        for (int i = 0; i < workingPointFree.Length; i++)
        {
            if (workingPointFree[i])
            {
                return true;
            }
        }
        return false;
    }

    public void GenerateWorkingPoints()
    {
        for (int i = 0; i < workingPoint.Count; i++)
        {
            DestroyImmediate(workingPoint[i].gameObject);
        }
        workingPoint.Clear();

        for (int i = 0; i < amountToSpawn; i++)
        {
            float angle = i * Mathf.PI * 2f / amountToSpawn;
            Vector3 newPos = transform.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            Vector3 dir =transform.position - newPos;
            RaycastHit hit;
            if (Physics.Raycast(newPos, dir.normalized, out hit, dir.magnitude, layerMask))
            {
                Debug.DrawLine(transform.position, hit.point - dir.normalized*entityRadius, Color.yellow, 5f);               
            }
            GameObject go = Instantiate(debugWorkingPointPrefab, newPos, Quaternion.identity);
            go.transform.parent = transform;
            Quaternion rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            go.transform.rotation = rotation;
            workingPoint.Add(go.transform);
        }
    }

}
