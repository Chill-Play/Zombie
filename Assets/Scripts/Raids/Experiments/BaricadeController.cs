using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaricadeController : MonoBehaviour
{
    public const string SECTION_SAVE_ID = "sections";

    public event System.Action<RaidBaricade, bool> OnBaricadeEnter;
    public event System.Action<RaidBaricade> OnBaricadeExit;

    PlayerTools playerTools;
    string[] sectionsArray;

    private void Awake()
    {
        sectionsArray = PlayerPrefs.GetString(SECTION_SAVE_ID, "").Split(new[] { "###" }, StringSplitOptions.None);

        playerTools = FindObjectOfType<PlayerTools>();
        ////////////////////////////////////////////////////////////////////////// refactor
        RaidBaricade[] raidBaricades = FindObjectsOfType<RaidBaricade>();
        for (int i = 0; i < raidBaricades.Length; i++)
        {
            bool exist = true;
            for (int j = 0; j < sectionsArray.Length; j++)
            {
                if (raidBaricades[i].SectionId != null && raidBaricades[i].SectionId.name == sectionsArray[j])
                {
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
    }

    private void BaricadeController_OnBaricadeExit(RaidBaricade obj)
    {
        OnBaricadeExit?.Invoke(obj);
    }

    private void BaricadeController_OnBaricadeEnter(RaidBaricade obj)
    {
        OnBaricadeEnter?.Invoke(obj, playerTools.CanUseTool(obj.ResourceTool));
    }
}
