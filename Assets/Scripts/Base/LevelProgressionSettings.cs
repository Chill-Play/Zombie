using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ZombieSim/Level Progression Settings")]
public class LevelProgressionSettings : ScriptableObject
{
    [System.Serializable]
    public struct ChestSettings
    {
        public ChestInfo chestInfo;
        public ResourcesInfo resourcesInfo;      
    }

    [SerializeField] List<ChestSettings> chestSettings = new List<ChestSettings>();
    [SerializeField] List<ResourceType> unlockResources = new List<ResourceType>();

    public List<ChestSettings> Chests => chestSettings;
    public List<ResourceType> UnlockResources => unlockResources;
}
