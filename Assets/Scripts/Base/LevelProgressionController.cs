using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionController : SingletonMono<LevelProgressionController>
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
        hq.OnRewardOpened += Hq_OnRewardOpened;
        cameraController = CameraController.Instance;
        resourcesController = ResourcesController.Instance;
        campGameplayController = CampGameplayController.Instance;
        inputPanel = InputPanel.Instance;
    }

    private void Start()
    {
        unlockableBuildingsArray = FindObjectsOfType<UnlockableBuilding>();
    }

    public void AddChestResources()
    {
        AdvertisementManager.Instance.TryShowInterstitial("claim_reward_chest");
        ResourcesInfo resInfo = LevelProgressionController.Instance.CurrentLevelProgression.Chests[hq.NextChest - 1].resourcesInfo;
        resourcesController.AddResources(resInfo);
        resourcesController.UpdateResources();
        CampSquad.Instance.MoveSurvivors();
    }

    private void Hq_OnLevelUp(int level)
    {       
        unlockableBuildings.Clear();
        foreach (var unlockableBuilding in unlockableBuildingsArray)
        {
            if (unlockableBuilding.CanUnlock(hq.Level))
            {
                unlockableBuildings.Add(unlockableBuilding);
            }
        }
        CampSquad.Instance.StopSurvivors();
    }

    void Hq_OnRewardOpened(int i)
    {
        CampSquad.Instance.StopSurvivors();
    }
    
    public void UnlockResourceType()
    {
        ResourcesController resourcesController = ResourcesController.Instance;
        List<ResourceType> unlockResources = CurrentLevelProgression.UnlockResources;
        foreach (var resourceType in unlockResources)
        {
            resourcesController.AddResourceType(resourceType);
        }
        resourcesController.UpdateResources();
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
        AdvertisementManager.Instance.TryShowInterstitial("hq_level_up");
        cameraController.SetTarget(campGameplayController.PlayerInstance);    
        inputPanel.EnableInput();
        CampSquad.Instance.MoveSurvivors();
    }
}
