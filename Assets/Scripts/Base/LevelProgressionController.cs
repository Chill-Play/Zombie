using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionController : MonoBehaviour
{
    [SerializeField] List<LevelProgressionSettings> levelProgressionSettings = new List<LevelProgressionSettings>();

    HQBuilding hq;

    public LevelProgressionSettings CurrentLevelProgression => levelProgressionSettings[hq.Level];

    private void Awake()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnLevelUp += Hq_OnLevelUp;
    }

    private void Hq_OnLevelUp()
    {
        ResourcesController resourcesController = FindObjectOfType<ResourcesController>();
        List<ResourceType> unlockResources = CurrentLevelProgression.UnlockResources;
        foreach (var resourceType in unlockResources)
        {
            resourcesController.AddResourceType(resourceType);
        }
        resourcesController.UpdateResources();
    }
}
