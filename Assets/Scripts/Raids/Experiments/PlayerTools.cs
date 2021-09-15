using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    ResourcesController resourcesController;
    SquadBackpack squadBackpack;

    private void Awake()
    {
        resourcesController = FindObjectOfType<ResourcesController>();
        squadBackpack = FindObjectOfType<SquadBackpack>();
    }


    public bool CanUseTool(ResourceType resourceType)
    {
        var resourcesCount = resourcesController.ResourcesCount;
        if ((resourcesCount.ContainsResourceType(resourceType) && resourcesCount.Count(resourceType) >= 1) || 
            (squadBackpack.HasResource(resourceType) && squadBackpack.Resources[resourceType] >= 1))
        {
            return true;
        }
        return false;
    }

    public void UseTool(ResourceType resourceTool)
    {
        Resource tool = Instantiate(resourceTool.defaultPrefab, transform.position + Vector3.up*3f, Quaternion.identity);
        IPlayerTool playerTool = tool.GetComponent<IPlayerTool>();
        playerTool.UseTool();
    }
}
