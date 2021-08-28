using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSquad : MonoBehaviour
{
    [SerializeField] GameObject campSurvivorPrefab;
    [SerializeField] float radius = 2f;

    int count;

    public void SpawnSquad(Vector3 spawnPoint)
    {
        BaseBuilding[] baseBuildings = FindObjectsOfType<BaseBuilding>();
        List<BaseBuilding> buildingsBuild = new List<BaseBuilding>();
        for (int i = 0; i < baseBuildings.Length; i++)
        {
            Buildable buildable = baseBuildings[i].GetComponent<Buildable>();
            if (buildable != null && !buildable.Built)
            {
                continue;
            }
            buildingsBuild.Add(baseBuildings[i]);
        }

        count = PlayerPrefs.GetInt("M_Survivors_Count", 0);

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector3 newPos = spawnPoint + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate<GameObject>(campSurvivorPrefab, newPos, Quaternion.identity);
            int target = Random.Range(0, buildingsBuild.Count);
            go.GetComponent<UnitMovement>().MoveTo(buildingsBuild[target].transform.position);
            go.GetComponent<CampSurvivor>().OnReachBuilding += CampSquad_OnReachBuilding;
        }
    }

    private void CampSquad_OnReachBuilding(CampSurvivor obj)
    {
        count--;
        PlayerPrefs.SetInt("M_Survivors_Count", count);
    }
}
