using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianController : MonoBehaviour
{
    CivilianBuilding[] civilianBuildings;
    Civilian[] civilians;
    Raid raid;

    private void Awake()
    {
        raid = FindObjectOfType<Raid>();
        raid.OnHordeBegin += CivilianController_OnHordeBegin;
        raid.OnHordeDefeated += Raid_OnHordeDefeated;
        civilians = FindObjectsOfType<Civilian>();
    }

    private void Start()
    {
        for (int i = 0; i < civilians.Length; i++)
        {
            civilians[i].GoIdle();
        }
    }

    private void Raid_OnHordeDefeated()
    {
        civilianBuildings = FindObjectsOfType<CivilianBuilding>();
        for (int i = 0; i < civilianBuildings.Length; i++)
        {
            civilianBuildings[i].SpawnCivilians();
        }
    }

    private void CivilianController_OnHordeBegin()
    {
        civilianBuildings = FindObjectsOfType<CivilianBuilding>();
        for (int i = 0; i < civilians.Length; i++)
        {
            int minIdx = -1;
            float minDist = float.MaxValue;
            for (int j = 0; j < civilianBuildings.Length; j++)
            {
                float dist = Vector3.Distance(civilians[i].transform.position, civilianBuildings[j].DoorPoint.position);
                if (dist < minDist)
                {
                    minIdx = j;
                    minDist = dist;
                }
            }
            civilians[i].GoToBuilding(civilianBuildings[minIdx]);
        }
      
    }
}
