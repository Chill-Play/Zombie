using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSurvivor : MonoBehaviour
{
    [SerializeField] UnitMovement unitMovement;
    [SerializeField] ResourceType resourceType;


    public event System.Action<CampSurvivor> OnReachBuilding;

    void Update()
    {
        if (unitMovement.IsReachDestination)
        {
            ResourcesInfo info = new ResourcesInfo();
            info.AddSlot(resourceType, 1);
            ResourcesController.Instance.AddResources(info);
            ResourcesController.Instance.UpdateResources();
            FindObjectOfType<UINumbers>().SpawnNumber(transform.position, "+1", Vector2.up * 2f, 0f, 0f, 1f);
            OnReachBuilding?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
