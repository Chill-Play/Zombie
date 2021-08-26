using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceBarController : MonoBehaviour
{
    [SerializeField] ResourcesInfoUIPanel panel;
    // Start is called before the first frame update
    void Start()
    {
        ResourcesController.Instance.OnResourcesUpdated += ResourcesController_OnResourcesUpdated;
        UpdateBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateBar()
    {
        var count = ResourcesController.Instance.ResourcesCount;
        foreach(var slot in count.Slots)
        {
            panel.UpdateBar(slot.type, slot.count);
        }
    }


    private void ResourcesController_OnResourcesUpdated()
    {
        UpdateBar();
    }
}
