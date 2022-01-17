using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCounter : MonoBehaviour
{
    public event System.Action OnRequireUpdate;
   
    [SerializeField] protected Transform uiPoint;

    protected ResourcesController resourcesController;

    public Transform UIPoint => uiPoint;

    protected virtual void Awake()
    {
        resourcesController = FindObjectOfType<ResourcesController>();
        resourcesController.OnResourcesUpdated += ResourcesController_OnResourcesUpdated;
    }

    protected void ResourcesController_OnResourcesUpdated()
    {
        RequireUpdate();
    }

    public virtual int AvailableUpgrades()
    {
        return 0;
    }

    protected void RequireUpdate()
    {
        OnRequireUpdate?.Invoke();
    }
}
