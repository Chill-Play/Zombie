using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianBuilding : MonoBehaviour
{
    [SerializeField] Transform doorPoint;
    [SerializeField] Civilian civilianPrefab;
    [SerializeField] List<Transform> civilianPoints = new List<Transform>();
    [SerializeField] float spawnRate = 0.5f;

    public Transform DoorPoint => doorPoint;

    public void SpawnCivilians()
    {
        StartCoroutine(SpawnCiviliansCoroutine());
    }

    IEnumerator SpawnCiviliansCoroutine()
    {
        for (int i = 0; i < civilianPoints.Count; i++)
        {
            Civilian civilian = Instantiate<Civilian>(civilianPrefab, doorPoint.position, doorPoint.rotation);
            civilian.GoDance(civilianPoints[i]);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
