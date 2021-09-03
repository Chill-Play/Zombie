using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorialHelper : MonoBehaviour
{
    [SerializeField] ConditionTrigger enableRaids;

    private void Awake()
    {
        enableRaids.OnTrigger += EnableRaids_OnTrigger;       
    }

    private void EnableRaids_OnTrigger()
    {
        RaidZone raidZone = FindObjectOfType<RaidZone>(true);
        raidZone.gameObject.SetActive(true);
        FindObjectOfType<CampGameplayController>().SetPlayerReturnedToRaidZone(true);
    }
}
