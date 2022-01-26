using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampTutorialHelper : MonoBehaviour
{
    [SerializeField] ConditionTrigger enableRaids;
    [SerializeField] private RaidZone raidZone;

    private void Awake()
    {
        enableRaids.OnTrigger += EnableRaids_OnTrigger;       
    }

    private void EnableRaids_OnTrigger()
    {
        raidZone.gameObject.SetActive(true);
        CampGameplayController.Instance.SetPlayerReturnedToRaidZone(true);
    }
}
