using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionController : MonoBehaviour
{
    [SerializeField] List<LevelProgressionSettings> levelProgressionSettings = new List<LevelProgressionSettings>();

    HQBuilding hq;

    private ResourcesController resourcesController;
    public LevelProgressionSettings CurrentLevelProgression => levelProgressionSettings[hq.Level];

    private void Awake()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnLevelUp += Hq_OnLevelUp;
    }

    private void Start()
    {
        resourcesController = FindObjectOfType<ResourcesController>();
    }

    public void AddChestResources()
    {
        ResourcesInfo resInfo = FindObjectOfType<LevelProgressionController>().CurrentLevelProgression.Chests[hq.NextChest - 1].resourcesInfo;
        resourcesController.AddResources(resInfo);
        resourcesController.UpdateResources();
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
