using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionController : MonoBehaviour
{
    [SerializeField] List<LevelProgressionSettings> levelProgressionSettings = new List<LevelProgressionSettings>();

    HQBuilding hq;
    private ResourcesController resourcesController;
    List<UnlockableBuilding> unlockableBuildings = new List<UnlockableBuilding>();
    UnlockableBuilding[] unlockableBuildingsArray;
    CameraController cameraController;
    CampGameplayController campGameplayController;
    InputPanel inputPanel;

    public LevelProgressionSettings CurrentLevelProgression => levelProgressionSettings[hq.Level];
    public List<UnlockableBuilding> UnlockableBuildings => unlockableBuildings;

    private void Awake()
    {
        hq = FindObjectOfType<HQBuilding>();
        hq.OnLevelUp += Hq_OnLevelUp;
        cameraController = FindObjectOfType<CameraController>();
        resourcesController = FindObjectOfType<ResourcesController>();
        campGameplayController = FindObjectOfType<CampGameplayController>();
        inputPanel = FindObjectOfType<InputPanel>();
    }

    private void Start()
    {
        unlockableBuildingsArray = FindObjectsOfType<UnlockableBuilding>();
       
    }

    public void AddChestResources()
    {
        ResourcesInfo resInfo = FindObjectOfType<LevelProgressionController>().CurrentLevelProgression.Chests[hq.NextChest - 1].resourcesInfo;
        resourcesController.AddResources(resInfo);
        resourcesController.UpdateResources();
    }

    private void Hq_OnLevelUp()
    {       
        unlockableBuildings.Clear();
        foreach (var unlockableBuilding in unlockableBuildingsArray)
        {
            if (unlockableBuilding.CanUnlock(hq.Level))
            {
                unlockableBuildings.Add(unlockableBuilding);
            }
        }
        UnlockBuildings();
        /*
        ResourcesController resourcesController = FindObjectOfType<ResourcesController>();
        List<ResourceType> unlockResources = CurrentLevelProgression.UnlockResources;
        foreach (var resourceType in unlockResources)
        {
            resourcesController.AddResourceType(resourceType);
        }
        resourcesController.UpdateResources();*/
    }

    public void UnlockBuildings()
    {
        StartCoroutine(UnlockBuildingsCoroutine());
    }

    IEnumerator UnlockBuildingsCoroutine()
    {
        inputPanel.DisableInput();
        for (int i = 0; i < unlockableBuildings.Count; i++)
        {
            bool waiting = true;
            cameraController.SetTarget(unlockableBuildings[i].transform, 0.5f, () => waiting = false);
            while (waiting)
            {
                yield return new WaitForEndOfFrame();
            }
            waiting = true;
            unlockableBuildings[i].UnlockWhithAnimation(() => waiting = false);
            while (waiting)
            {
                yield return new WaitForEndOfFrame();
            }           
        }
        cameraController.SetTarget(campGameplayController.PlayerInstance);
        inputPanel.EnableInput();
    }
}
