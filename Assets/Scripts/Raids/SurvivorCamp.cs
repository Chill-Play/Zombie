using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorCamp : MonoBehaviour
{
    [SerializeField] GameObject survivorPrefab;
    [SerializeField] float timeBeforeFirstSpawn = 1.5f;
    [SerializeField] float spawnRate = 1.5f;
    [SerializeField] Transform spawnPoint;
    [SerializeField] List<Transform> points = new List<Transform>();
    Coroutine spawnCoroutine;

   

    private void OnEnable()
    {
        spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeFirstSpawn);
       
        for (int i = 0; i < points.Count; i++)
        {
            GameObject survivor = Instantiate<GameObject>(survivorPrefab, spawnPoint.position, spawnPoint.rotation);
            survivor.GetComponent<UnitMovement>().MoveTo(points[i].position);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(spawnCoroutine);
    }
}
