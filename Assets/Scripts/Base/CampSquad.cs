using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CampSquad : SingletonMono<CampSquad>
{
    [SerializeField] GameObject campSurvivorPrefab;
    [SerializeField] float radius = 2f;

    private List<CampSurvivor> survivors = new List<CampSurvivor>();
    int count;
    

    public void SpawnSquad(Vector3 spawnPoint)
    {
        count = PlayerPrefs.GetInt("M_Survivors_Count", 0);
        HQBuilding hq = FindObjectOfType<HQBuilding>();

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 newPos = spawnPoint + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(campSurvivorPrefab, newPos, Quaternion.identity);
            Vector3 dir = hq.transform.position - transform.position;
            go.GetComponent<UnitMovement>().MoveTo(hq.transform.position - dir.normalized * 2f);
            CampSurvivor survivor = go.GetComponent<CampSurvivor>();
            survivor.OnReachBuilding += CampSquad_OnReachBuilding;
            survivor.SetDestination(hq.transform.position - dir.normalized * 2f);
            survivors.Add(survivor);
        }
    }

    private void CampSquad_OnReachBuilding(CampSurvivor obj)
    {
        count--;      
        PlayerPrefs.SetInt("M_Survivors_Count", count);
        survivors.Remove(obj);
    }

    public void MoveSurvivors()
    {
        foreach (var survivor in survivors)
        {
            survivor.ForceToMove();
        }
    }
    
    public void StopSurvivors()
    {
        foreach (var survivor in survivors)
        {
            survivor.ForceToStop();
        }
    }
}
