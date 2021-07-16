using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapPlayerController : MonoBehaviour
{
    Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                StateArea stateArea = hit.collider.GetComponent<StateArea>();
                if (stateArea != null)
                {
                    stateArea.LoadStateBase();
                }
            }
        }
    }

}
