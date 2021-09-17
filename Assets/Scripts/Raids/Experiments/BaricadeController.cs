using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaricadeController : MonoBehaviour
{
    public const string SECTION_SAVE_ID = "sections";

    public event System.Action<RaidBaricade, bool> OnBaricadeEnter;
    public event System.Action<RaidBaricade> OnBaricadeExit;

    [SerializeField] SubjectId defaultSection;


    ZombieSpawnPointPack[] zombieSpawnPointPacks;

    PlayerTools playerTools;
    string[] sectionsArray;
    Level level;
    SpawnPoint[] spawnPoints;

    SubjectId currentSection;

    public SpawnPoint CurrentSpawnPoint { get; set; }

    public SubjectId CurrentSection => currentSection;

    private void Awake()
    {
        sectionsArray = PlayerPrefs.GetString(SECTION_SAVE_ID, "").Split(new[] { "###" }, StringSplitOptions.None);
        playerTools = FindObjectOfType<PlayerTools>();
        spawnPoints = FindObjectsOfType<SpawnPoint>(true);
        zombieSpawnPointPacks = FindObjectsOfType<ZombieSpawnPointPack>();    

        level = FindObjectOfType<Level>();        

        ////////////////////////////////////////////////////////////////////////// refactor
        RaidBaricade[] raidBaricades = FindObjectsOfType<RaidBaricade>();
        for (int i = 0; i < raidBaricades.Length; i++)
        {
            bool exist = true;         
            for (int j = 0; j < sectionsArray.Length; j++)
            {
                if (raidBaricades[i].SectionId != null && raidBaricades[i].SectionId.name == sectionsArray[j])
                {
                    if (j >= 1 && j == sectionsArray.Length - 1)
                    {
                        currentSection = raidBaricades[i].NextSectionId;
                    }
                    exist = false;
                    break;
                }
            }
            
            if (!exist)
            {
                Destroy(raidBaricades[i].gameObject);               
                continue;
            }
            raidBaricades[i].OnBaricadeEnter += BaricadeController_OnBaricadeEnter;
            raidBaricades[i].OnBaricadeExit += BaricadeController_OnBaricadeExit;
            raidBaricades[i].OnDead += BaricadeController_OnDead;
        }

        if (currentSection == null)
        {
            currentSection = defaultSection;
        }
        
        for (int i = 0; i < zombieSpawnPointPacks.Length; i++)
        {           
            if (zombieSpawnPointPacks[i].SectionId == currentSection)
            {
                level.SetZombiesSpawnPoint(zombieSpawnPointPacks[i].SpawnPoints);
                break;               
            }
        }


        for (int i = 0; i < spawnPoints.Length; i++)
        {
            for (int j = 0; j < sectionsArray.Length; j++)
            {
                if (spawnPoints[i].SectionId != null && spawnPoints[i].SectionId.name == sectionsArray[j])
                {                   
                    spawnPoints[i].SetVisable(false);
                    break;
                }
            }
            if (currentSection!= null && spawnPoints[i].SectionId == currentSection)
            {
                CurrentSpawnPoint = spawnPoints[i];
                spawnPoints[i].SetVisable(true);
            }
        }     

        FindObjectOfType<Squad>().transform.position = CurrentSpawnPoint.transform.position;
        FindObjectOfType<PlayerTools>().transform.position = CurrentSpawnPoint.transform.position;

        PickupItem[] pickupItems = FindObjectsOfType<PickupItem>();
        for (int i = 0; i < pickupItems.Length; i++)
        {
            bool exist = true;
            for (int j = 0; j < sectionsArray.Length; j++)
            {
                if (pickupItems[i].SectionId != null && pickupItems[i].SectionId.name == sectionsArray[j])
                {
                    exist = false;
                    break;
                }
            }
            if (!exist)
            {
                Destroy(pickupItems[i].gameObject);               
                continue;
            }
        }

        SurvivorPickup[] survivorPickups = FindObjectsOfType<SurvivorPickup>();
        for (int i = 0; i < survivorPickups.Length; i++)
        {            
            for (int j = 0; j < sectionsArray.Length; j++)
            {
                if (survivorPickups[i].SectionId != null && survivorPickups[i].SectionId.name == sectionsArray[j])
                {
                    Destroy(survivorPickups[i].gameObject);
                    break;
                }
            }
        }

        ResourceSpot[] resourceSpots = FindObjectsOfType<ResourceSpot>();
        for (int i = 0; i < resourceSpots.Length; i++)
        {
            bool exist = true;
            for (int j = 0; j < sectionsArray.Length; j++)
            {
                if (resourceSpots[i].SectionId != null && resourceSpots[i].SectionId.name == sectionsArray[j])
                {
                    exist = false;
                    break;
                }
            }
            if (!exist)
            {
                if (!resourceSpots[i].ExistInOpenSection)
                {                  
                    Destroy(resourceSpots[i].gameObject);
                }
                else
                {
                    resourceSpots[i].NullNoise();
                }
                continue;
            }
        }

        ////////////////////////////////////////////////////////////////////////// refactor
    }

    private void BaricadeController_OnDead(EventMessage<Empty> obj)
    {
        List<string> openedSections = new List<string>();
        openedSections.AddRange(sectionsArray);
        RaidBaricade baricade = (RaidBaricade)obj.sender;
        openedSections.Add(baricade.SectionId.name);
        PlayerPrefs.SetString(SECTION_SAVE_ID, string.Join("###", openedSections));
        currentSection = baricade.NextSectionId;

        CurrentSpawnPoint.IsReturningToBase = false;
        CurrentSpawnPoint.Hide();

        for (int i = 0; i < spawnPoints.Length; i++)
        {      
            if (currentSection != null && spawnPoints[i].SectionId == currentSection)
            {
                CurrentSpawnPoint = spawnPoints[i];
                CurrentSpawnPoint.Show();
                break;
            }
        }    

        for (int i = 0; i < zombieSpawnPointPacks.Length; i++)
        {
            if (zombieSpawnPointPacks[i].SectionId == currentSection)
            {
                level.SetZombiesSpawnPoint(zombieSpawnPointPacks[i].SpawnPoints);
                break;
            }
        }
        level.ResetNoise();
    }

    private void BaricadeController_OnBaricadeExit(RaidBaricade obj)
    {
        OnBaricadeExit?.Invoke(obj);
    }

    private void BaricadeController_OnBaricadeEnter(RaidBaricade obj)
    {
        OnBaricadeEnter?.Invoke(obj, playerTools.CanUseTool(obj.ResourceTool) && level.SectionClear);
    }
}
