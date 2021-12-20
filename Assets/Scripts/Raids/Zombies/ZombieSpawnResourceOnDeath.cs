using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnResourceOnDeath : MonoBehaviour
{
    [SerializeField] UnitHealth health;
    [SerializeField] ResourceType resourceType;
    [SerializeField] float resourcesVelocity = 1f;
    [SerializeField] Vector3 resourceSpawnOffset = new Vector3(0f, 1f, 0f);
    // Start is called before the first frame update
    void OnEnable()
    {
        health.OnDead += Health_OnDead;
    }

    private void Health_OnDead(EventMessage<Empty> obj)
    {
        SpawnResource();
    }

    private void OnDisable()
    {
        health.OnDead -= Health_OnDead;
    }


    // Update is called once per frame
    void SpawnResource()
    {
        Resource instance = Instantiate(resourceType.defaultPrefab, transform.position + resourceSpawnOffset, transform.rotation);
        instance.Pickup(FindObjectOfType<Squad>().Units[0].transform);
        Rigidbody body = instance.GetComponent<Rigidbody>();
        body.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(3f, 6f), Random.Range(-1f, 1f)) * resourcesVelocity;
        body.angularVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 360f;
    }
}
