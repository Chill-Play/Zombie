using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CampSquad : MonoBehaviour
{
    [SerializeField] GameObject campSurvivorPrefab;
    [SerializeField] float radius = 2f;
    [SerializeField] BaseBuilding targetBuilding;

    int count;
    Tween punchTween;

    public void SpawnSquad(Vector3 spawnPoint)
    {
        //BaseBuilding[] baseBuildings = FindObjectsOfType<BaseBuilding>();
        //List<BaseBuilding> buildingsBuild = new List<BaseBuilding>();
        //for (int i = 0; i < baseBuildings.Length; i++)
        //{
        //    Buildable buildable = baseBuildings[i].GetComponent<Buildable>();
        //    if (buildable != null && !buildable.Built)
        //    {
        //        continue;
        //    }
        //    buildingsBuild.Add(baseBuildings[i]);
        //}

        count = PlayerPrefs.GetInt("M_Survivors_Count", 0);

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 newPos = spawnPoint + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(campSurvivorPrefab, newPos, Quaternion.identity);
            Vector3 dir = targetBuilding.transform.position - transform.position;
            go.GetComponent<UnitMovement>().MoveTo(targetBuilding.transform.position - dir.normalized * 2f);
            go.GetComponent<CampSurvivor>().OnReachBuilding += CampSquad_OnReachBuilding;
        }
    }

    private void CampSquad_OnReachBuilding(CampSurvivor obj)
    {
        count--;
        PlayerPrefs.SetInt("M_Survivors_Count", count);
        targetBuilding.transform.localScale = Vector3.one;
        if(punchTween != null)
        {
            punchTween.Complete();
        }
        punchTween = targetBuilding.transform.DOPunchScale(Vector3.one * 0.05f, 0.2f);
    }
}
