using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIndicatorUI : MonoBehaviour
{
    [SerializeField] float radius = 300;
    [SerializeField] Transform house;
    [SerializeField] GameObject group;
    
    SpawnPoint spawnPoint;
    GameObject squad;

    Canvas canvas;
    RectTransform canvasRect;

    private void Start()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();
        squad = FindObjectOfType<Squad>().gameObject;
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }


    void Update()
    {
        UpdateIndicator();
    }

    public void UpdateIndicator()
    {
        Vector3 spawnPointScreenPosition = CameraController.Instance.Camera.WorldToScreenPoint(spawnPoint.transform.position);
        bool insideCanvas = RectTransformUtility.RectangleContainsScreenPoint(canvasRect, spawnPointScreenPosition);

        if (!insideCanvas)
        {
            if (!group.activeSelf)
                group.SetActive(true);
        }
        else
        {
            if (group.activeSelf)
                group.SetActive(false);
        }

        Vector3 direction = spawnPoint.transform.position - squad.transform.position;
        direction.Normalize();
        Vector3 screenDirection = new Vector3(direction.x, direction.z, 0f);
        transform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0) + screenDirection * (radius * canvas.scaleFactor);
        transform.right = screenDirection;
        house.right = Vector3.right;
    }
}
